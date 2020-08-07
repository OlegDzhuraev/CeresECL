﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CeresECL
{
    public sealed class Logics : Container
    {
        readonly Logic[] logics = new Logic[CeresSettings.MaxEntityLogics];
        int logicsCount;

        readonly List<object> injects = new List<object>();

        public Logics(Entity entity) : base(entity) { }
        
        public override void Run()
        {
            for (int i = 0; i < logicsCount; i++)
                if (logics[i] is IRunLogic runLogic)
                    runLogic.Run();
        }

        public void Add<T>() where T : Logic, new()
        {
            if (logicsCount == CeresSettings.MaxEntityLogics)
            {
                Debug.LogWarning("You're trying to add more logics than it is allowed. Change CeresSettings parameters to allow it.");
                return;
            }
            
            for (var i = 0; i < logicsCount; i++)
                if (logics[i] is T)
                    return;

            var newLogic = new T
            {
                Entity = Entity
            };

            for (var i = 0; i < injects.Count; i++)
                Inject(newLogic, injects[i]);
            
            if (newLogic is IInitLogic initLogic)
                initLogic.Init();

            logics[logicsCount] = newLogic;
            logicsCount++;
        }

        void Inject(Logic inLogic, object data)
        {
            var dataType = data.GetType();

            var bindingAttribute = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var ignoreAttribute = typeof(CeresIgnoreInjectAttribute); ;

            var logicType = inLogic.GetType();

            foreach (var field in logicType.GetFields(bindingAttribute))
            {
                if (field.IsStatic || Attribute.IsDefined(field, ignoreAttribute))
                    continue;

                if (field.FieldType.IsAssignableFrom(dataType))
                {
                    field.SetValue(inLogic, data);
                    break;
                }
            }
        }
           
        /// <summary> Method allows you inject any data to all Entity logics. </summary>
        public void Inject(object data)
        {
            if (!injects.Contains(data))
                injects.Add(data);

            for (var i = 0; i < logicsCount; i++)
                Inject(logics[i], data);
        }
        
        /// <summary> Used only for editor scripting. </summary>
        public List<Logic> GetListEditor()
        {
            var list = new List<Logic>();
            
            for (var i = 0; i < logicsCount; i++)
                list.Add(logics[i]);
            
            return list;
        }
    }
}