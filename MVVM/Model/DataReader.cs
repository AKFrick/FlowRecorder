using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NModbus;
using NModbus.IO;
using System.Net.Sockets;
using NModbus.Utility;
using NModbus.Device;
using System.Threading;
using System.IO;

namespace FlowRecorder.MVVM.Model
{
    public class DataReader
    {
        ModbusFactory factory;
        Task readingTask;
        bool StopReadingSetted;

        CancellationTokenSource tokenSource2;
        CancellationToken ct;

        public DataReader()
        {
            factory = new();
        }

        byte slaveID = 16;
        ushort startAddress = 0;
        ushort numOfPoints = 6;

        public event Action<Double> ValueRead;

        public async void StartReading()
        {
            tokenSource2 = new();
            ct = tokenSource2.Token;

            readingTask = Task.Run(() =>
            {
                OutputLog.That("Подключение к MOXA ");

                using TcpClient tcpClient = new();
                tcpClient.Connect("192.168.10.254", 4001);

                using TcpClientAdapter adapter = new(tcpClient) { ReadTimeout = 3000 };

                using ModbusSerialMaster master = (ModbusSerialMaster)factory.CreateRtuMaster(adapter);
                OutputLog.That("Подключение установлено. Начинаем читать");

                while (true)
                {
                    ushort[] holding_register = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);

                    ValueRead?.Invoke(ModbusUtility.GetSingle(holding_register[4], holding_register[5]));

                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(1000);
                }

            }, ct);

            try
            {
                await readingTask;
            }
            catch (SocketException ex)
            {
                OutputLog.That($"Ошибка соединения {ex.Message}");

                RetryConnect();
            }
            catch (IOException ex)
            {
                OutputLog.That($"Соединение разорвано {ex.Message}");

                RetryConnect();
            }
            catch (OperationCanceledException ex)
            {
                OutputLog.That($"Чтение прервано пользователем!");
            }
            catch (Exception ex)
            {
                OutputLog.That($"{ex.Message} : {ex.GetType()}");
            }
            finally
            {
                tokenSource2.Dispose();
            }
        }

        public void StopReading()
        {
            tokenSource2.Cancel();
        }

        async void RetryConnect()
        {
            tokenSource2 = new();
            ct = tokenSource2.Token;

            OutputLog.That("Пауза перед повторной попыткой соединения");

            try
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < 60; i++)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        Thread.Sleep(1000);
                    }
                }, ct);
            }
            catch (OperationCanceledException ex)
            {
                OutputLog.That($"Чтение прервано пользователем!");
            }
            finally
            {
                tokenSource2.Dispose();
            }

            StartReading();
        }
    }
}
