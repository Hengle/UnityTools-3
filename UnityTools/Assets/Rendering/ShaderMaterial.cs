using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Rendering;

namespace UnityTools.Common
{
    [System.Serializable]
    public class ShaderMaterial<T>: ShaderMaterial where T : Enum
    {
        public T type;
    }
	[System.Serializable]
	public class ShaderMaterial : IInitialize
	{
		public string name;
		public Shader shader;
		public DisposableMaterial material;

		public bool Inited => this.inited;
        protected bool inited = false;


		public void Init(params object[] parameters)
        {
            if(this.Inited) return;

            this.material?.Dispose();
            this.material = new DisposableMaterial(this.shader);
            this.inited = true;
        }
		public void Deinit(params object[] parameters)
		{
            this.material?.Dispose();
			this.inited = false;
		}
	}
}