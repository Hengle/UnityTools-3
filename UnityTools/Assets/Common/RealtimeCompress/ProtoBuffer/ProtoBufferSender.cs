﻿using Google.Protobuf;
using UnityEngine;
using UnityTools.Common;
using UnityTools.Rendering;

namespace UnityTools.Networking
{
    public class ProtoBufferSender : AsyncGPUDataReader
    {
        // Start is called before the first frame update

        protected UDPTextureSocket sender = new UDPTextureSocket();
        [SerializeField] protected RenderTexture target;
        void Start()
        {
            this.target = TextureManager.Create(new RenderTextureDescriptor(256, 256));
            var camera = this.GetComponent<Camera>();
            camera.targetTexture = this.target;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            var temp = RenderTexture.GetTemporary(target.width, target.height, 0, RenderTextureFormat.ARGB32);
            {
                Graphics.Blit(target, temp);
                this.QueueTexture(temp);
            }
            RenderTexture.ReleaseTemporary(temp);
        }

        protected override void OnSuccessed(FrameData frame)
        {
            var readback = frame.readback;

            var data = readback.GetData<byte>().ToArray();

            var fileData = new ImageFile.FileData();
            fileData.Parameter = new ImageFile.Parameter();
            fileData.Parameter.Width = readback.width;
            fileData.Parameter.Height = readback.height;
            fileData.Data = ByteString.CopyFrom(data);

            var socketData = SocketData.Make("localhost", 12345);

            this.sender.Send(socketData, fileData);
        }
    }
}