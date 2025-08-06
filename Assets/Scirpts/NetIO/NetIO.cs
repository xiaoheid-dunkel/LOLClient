using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class NetIO {


    //kuozhnag
    //public static void Write(this MonoBehaviour mono,byte type,int area,int  command,object message){
    //    Instance.write (type,area,command,message);
    //}

    public static NetIO instance;//单例

    public List<SocketModel> messages = new List<SocketModel>();

    private Socket socket;
    
     private string ip = "127.0.0.1";
   // private string ip = "10.10.10.23";
    private int port = 6650;
    private  byte[] readbuff=new byte[1024];
    private bool isReading = false;

    List<byte> cache = new List<byte>();
   
   /// <summary>
   /// 单例对象
   /// </summary>
   public static NetIO Instance
   {
       get
       {
           if (instance == null)
           {
               instance = new NetIO();
           }
           return instance;
       }
   }

    private NetIO()
    {
        try
        {
            //创建客户端链接
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //链接服务器
            socket.Connect(ip, port);
            // 开启异步消息接收,消息到达后会直接写入缓冲区 readbuff；
            socket.BeginReceive(readbuff, 0, 1024, SocketFlags.None, ReceivecallBall,readbuff);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }     
    }
    //收到消息后回调
    private void ReceivecallBall(IAsyncResult ar)
    {
        try
        {
        //收到消息之后结束掉异步的接收,返回当前收到的消息的长度
          int length= socket.EndReceive(ar);
          byte[] message = new byte[length];

          Buffer.BlockCopy(readbuff ,0,message,0,length);
          cache.AddRange(message);
          if (!isReading)
          {
              isReading = true;
              OnDate();
          }
            //新添加的 
          //尾递归  再次开启异步消息接收,消息到达后会直接写入缓冲区 readbuff；
          socket.BeginReceive(readbuff, 0, 1024, SocketFlags.None, ReceivecallBall, readbuff);
        }
        catch(Exception e)
        {
            Debug.Log("远程服务器主动断开连接"+e.Message);
            socket.Close();
        }
       
    }
   
    //缓存中数据处
    public void OnDate()
    {
        
        //有数据的化调用长度解码
        byte[] result = decode(ref cache);

       //长度解码返回空的话，说明消息体不全，等待下条消息过来补全
        if (result==null)
        {
            isReading = false;
            return;
        }
        //消息解码
        SocketModel message = mdecode(result);

        if (message==null)
        {
            isReading = false;
            return;    
        }
        //进行消息的处理 存下来，等待Unity来调用。

        messages.Add(message);
       
        //尾递归，防止在消息处理过程中有其他消息到达而没有经过处理
         OnDate();
    }

    //消息长度解码
    public byte[] decode(ref  List<byte> cache)
    {
        //消息体的长度识别数据都不够，肯定没数据
        if (cache.Count < 4)   return null ;

        //创建内存流对象，并将缓存数据写进去
        MemoryStream ms = new MemoryStream(cache.ToArray());
        //二进制读取流
        BinaryReader br = new BinaryReader(ms);
        //从缓存中读取int型消息体长度
        int  length=br.ReadInt32();
        //如果消息体长度大于缓存中数据长度，说明消息没有读取完成 ，等待下次消息到达后再次处理。
        if (length>ms.Length-ms.Position)
        {
            return null;
        }
        //读取正确长度的数据
        byte[] result = br.ReadBytes(length);
        //清空缓存
        cache.Clear();

        //将读取后剩余的数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length-ms.Position)));
        br.Close();
        ms.Close();
        return result;
      
    }
    //消息解码
    public SocketModel mdecode(byte[] value)
    {
        ByteArray ba = new ByteArray(value);
        SocketModel model = new SocketModel();

        byte type;
        int area;
        int command;
        //从数据中读取三层协议，读取数据顺序必须和写入顺序保持一致，
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);      
        model.type = type;
        model.area = area;
        model.command = command;

        //判断读取玩协议后，是否还有数据需要读取，是则说明有消息体，进行消息体读取
        if (ba.Readnable)
        {

            byte[] message;
            //将剩余的数据全部读取出来
            ba.read(out message ,ba.Length-ba.Position);
            //反序列化对象
            model.message = SerializeUtil.decode(message);
        }
        ba.Close();
        return model;
    }

    //发送消息 调用的时候 NetIO.instance.write()
    public void write(byte type, int area, int command, object message)
    {
        //消息体编码。
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        if (message!=null)
        {
            ba.write(SerializeUtil.encode(message));
        }

       // 长度编码
        ByteArray  arr1 = new ByteArray();
        arr1.write(ba.Length);
        arr1.write(ba.getBuff());

        //发送
        try
        {
            socket.Send(arr1.getBuff());

        }
        catch (Exception e)
        {
            Debug.Log("网络错误，请重新登录"+e.Message);
        }
       
    }

     
}
