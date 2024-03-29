﻿using System;
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

        public DataReader(string Name, string Ip, int Port, byte SlaveID, DataToRead Data, int UpdateInterval)
        {
            factory = new();
            this.Name = Name;
            this.Ip = Ip;
            this.Port = Port;
            this.slaveID = SlaveID;  
            this.Data = Data;

            if (UpdateInterval > 20000)
            {
                this.UpdateInterval = 20000;
            }
            else if (UpdateInterval < 500)
            {
                this.UpdateInterval = 500;
            }
            else
            {
                this.UpdateInterval = UpdateInterval;
            }
        }

        public string Name { get; private set; }
        byte slaveID = 1;
        DataToRead Data { get; set; }
        string Ip;
        int Port;
        int UpdateInterval;

        public event Action<ushort[]> DataRead; //Сделать массивом, для того, чтобы считывать и расходомер и плотномер
        public event Action Connected;
        public event Action Disconnected;

        public async void StartReading()
        {
            tokenSource2 = new();
            ct = tokenSource2.Token;

            readingTask = Task.Run(() =>
            {
                OutputLog.That($"{Name}: Подключение к MOXA ");

                using TcpClient tcpClient = new();
                tcpClient.Connect(Ip, Port);

                using TcpClientAdapter adapter = new(tcpClient) { ReadTimeout = 3000 };

                using ModbusSerialMaster master = (ModbusSerialMaster)factory.CreateRtuMaster(adapter);
                OutputLog.That($"{Name}: Подключение установлено. Начинаем читать");

                Connected?.Invoke();

                while (true)
                {
                    ushort[] data = master.ReadInputRegisters(slaveID, Data.StartAddress, Data.NumOfPoint);

                    DataRead?.Invoke(data);


                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(UpdateInterval);
                }

            }, ct);

            try
            {
                await readingTask;
            }
            catch (SocketException ex)
            {
                OutputLog.That($"{Name}: Ошибка соединения {ex.Message}");

                RetryConnect();
            }
            catch (IOException ex)
            {
                OutputLog.That($"{Name}: Соединение разорвано {ex.Message}");

                RetryConnect();
            }
            catch (OperationCanceledException ex)
            {
                OutputLog.That($"{Name}: Чтение прервано пользователем!");
            }
            catch (Exception ex)
            {
                OutputLog.That($"{Name}: {ex.Message} : {ex.GetType()}");
                RetryConnect();
            }
            finally
            {
                Disconnected?.Invoke();
                tokenSource2.Dispose();
            }
        }

        public void StopReading()
        {
            try
            {
                tokenSource2.Cancel();
            }
            catch (Exception ex)
            {
                OutputLog.That($"{Name} {ex.Message}");
            }
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
