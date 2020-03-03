﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UnityTools.Common
{
    [Serializable]
    public class PCInfo
    {
        public string name = "OutputPC";
        public string ipAddress = "127.0.0.1";
        public bool isServer = false;
    }
    public class Launcher<T> : MonoBehaviour where T : class, new()
    {
        public interface ILauncherUser
        {
            void OnInit(T data);
            void OnDeinit(T data);
            void OnReload(T data);

            //higher order of user executes after than lower order user
            int Order { get; }
        }

        [SerializeField] protected bool global = false;
        [SerializeField] protected bool debug = false;
        [SerializeField] protected T data = new T();
        protected List<ILauncherUser> userList = new List<ILauncherUser>();

        protected virtual void OnEnable()
        {
            this.CleanUp();
            if (this.global)
            {
                foreach (var g in ObjectTool.FindRootObject())
                {
                    this.userList.AddRange(g.GetComponents<ILauncherUser>());
                    this.userList.AddRange(g.GetComponentsInChildren<ILauncherUser>());
                }
            }
            else
            {
                this.userList.AddRange(this.GetComponents<ILauncherUser>());
                this.userList.AddRange(this.GetComponentsInChildren<ILauncherUser>());
            }

            this.userList = this.userList.OrderBy(ul => ul.Order).ToList();
            foreach (var u in this.userList)
            {
                if (this.debug) Debug.Log("Init order " + u.Order + " " + u.ToString());
                u.OnInit(this.data);
            }
        }
        protected virtual void OnDisable()
        {
            foreach(var u in this.userList)
            {
                u.OnDeinit(this.data);
            }
            this.CleanUp();
        }

        protected virtual void OnReload()
        {
            foreach (var u in this.userList)
            {
                u.OnReload(this.data);
            }
        }

        protected void CleanUp()
        {
            this.userList.Clear();
        }
    }
}
