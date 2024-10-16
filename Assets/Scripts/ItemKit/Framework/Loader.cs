using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IItemKitLoader
    {
        ItemDatabase LoadItemDatabase(string fileName);
        ItemLanguagePackage LoadLanagePackage(string fileName);
        void LoadAsyncItemDatabase(string fileName, Action<ItemDatabase> cb);
        void LoadAsyncLanagePackage(string fileName, Action<ItemLanguagePackage> cb);
    }

    public class DefaultItemKitLoader : IItemKitLoader
    {
        public void LoadAsyncItemDatabase(string fileName, Action<ItemDatabase> cb)
        {
            var request = Resources.LoadAsync<ItemDatabase>(fileName);
            request.completed += _ => 
            {
                cb(request.asset as  ItemDatabase);
            };
        }

        public void LoadAsyncLanagePackage(string fileName, Action<ItemLanguagePackage> cb)
        {
            var request = Resources.LoadAsync<ItemLanguagePackage>(fileName);
            request.completed += _ =>
            {
                cb(request.asset as ItemLanguagePackage);
            };
        }

        public ItemDatabase LoadItemDatabase(string fileName)
        {
            return Resources.Load<ItemDatabase>(fileName);
        }

        public ItemLanguagePackage LoadLanagePackage(string fileName)
        {
            return Resources.Load<ItemLanguagePackage>(fileName);
        }
    }
}
