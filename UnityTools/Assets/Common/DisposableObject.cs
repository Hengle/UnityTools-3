﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityTools.Common
{
    public class DisposableObject<T> : Disposable
    {
        public T Data { get => this.data; set => this.data = value; }
        [SerializeField] protected T data;

        public DisposableObject(T data)
            : base()
        {
            this.data = data;
        }

    }

    public class DisposableRenderTexture : DisposableObject<RenderTexture>
    {
        public DisposableRenderTexture(RenderTexture data) : base(data)
        {
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            Assert.IsNotNull(this.data);
            data?.DestoryObj();
            data = null;
        }
        public static implicit operator DisposableRenderTexture(RenderTexture data)
        {
            return new DisposableRenderTexture(data);
        }
        public static implicit operator RenderTexture(DisposableRenderTexture source)
        {
            return source.Data;
        }
    }
    public class DisposableTexture2D : DisposableObject<Texture2D>
    {
        public DisposableTexture2D(Texture2D data) : base(data)
        {
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            Assert.IsNotNull(this.data);
            data?.DestoryObj();
            data = null;
        }
        public static implicit operator DisposableTexture2D(Texture2D data)
        {
            return new DisposableTexture2D(data);
        }
        public static implicit operator Texture2D(DisposableTexture2D source)
        {
            return source.Data;
        }
    }

}
