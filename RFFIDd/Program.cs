using MyReaderAPI;
using MyReaderAPI.MyInterface;
using MyReaderAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SampleCode
{
    class Program : IAsynchronousMessage
    {
        static String ConnID = "";

        static void Main(String[] args)
        {
            Program example = new Program();

            #region Connect
            Dictionary<string, string> dic_UsbDevicePath_Name = new Dictionary<string, string>();
            List<string> listUsbDevicePath = My_Reader.GetUsbHidDeviceList();
            Console.WriteLine("Please select USB connect parameter");
            try
            {
                if (0 < listUsbDevicePath.Count)
                {
                    int deviceIndex = 0;
                    IntPtr Handle = User32API.GetCurrentWindowHandle();

                    if (My_Reader.CreateUsbConn(listUsbDevicePath[deviceIndex], Handle, example))
                    {
                        ConnID = listUsbDevicePath[deviceIndex];
                        if (My_Reader.CheckConnect(ConnID))
                        {
                            Console.WriteLine("Connect success!\n");
                        }
                        else
                        {
                            Console.WriteLine("Connect failure!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Connect failure!\n");
                    }
                }
                else
                {
                    Console.WriteLine("Connect parameter select error!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            try
            {
                while (ConnID.Equals(""))
                {
                    Thread.Sleep(3000);
                }
    
                #region Read 6C Tags
                Console.WriteLine("Read 6C Tag");
                Console.WriteLine("GetEPC(String ConnID, eAntennaNo antNum, eReadType readType)");
                
                eAntennaNo antNum = new eAntennaNo();
                
                int theNum = 1;
                
                Console.WriteLine("Antenna number: ");
                Console.WriteLine(theNum);

                if (theNum == 1)
                {
                    antNum |= eAntennaNo._1;
                } else if (theNum == 2)
                {
                    antNum |= eAntennaNo._2;
                } else if (theNum == 3)
                {
                    antNum |= eAntennaNo._3;
                }

                eReadType readType;
                Console.WriteLine("eReadType.Single");
                
                readType = eReadType.Single;

                if (My_Reader._Tag6C.GetEPC(ConnID, antNum, readType) == 0)
                {
                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("Failure!");
                }
                #endregion

                My_Reader._Config.Stop(ConnID);
                My_Reader.CloseConn(ConnID);
                return;
            }       
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Interface

        public void WriteDebugMsg(String msg) 
        {
	    	
	    }

	    public void WriteLog(String msg) 
        {
	    	
	    }
        
	    public void PortConnecting(String connID) 
        {
            Console.WriteLine(connID);            
            if (My_Reader.GetServerStartUp())
            {
                Console.WriteLine("A reader connected to this server: " + connID);
                ConnID = connID;
            }
	    }

	    public void PortClosing(String connID) 
        {
	    	
	    }

	    public void OutPutTags(Tag_Model tag) 
        {	    	
	    	Console.WriteLine("EPC："+ tag.EPC + " TID：" + tag.TID + " ReaderName：" + tag.ReaderName + " Time:" + tag.ReadTime);
	    }

	    public void OutPutTagsOver() 
        {
	    	
	    }

	    public void GPIControlMsg(GPI_Model gpi_model) 
        {
            Console.WriteLine("gpiindex: " + gpi_model.GpiIndex + " gpistate: " + gpi_model.GpiState + " StartOrStop: " + gpi_model.StartOrStop + " Time: " + gpi_model.UtcTime);
        }

        #endregion
    }
}
