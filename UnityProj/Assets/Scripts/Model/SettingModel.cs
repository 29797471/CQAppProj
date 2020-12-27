using UnityEngine;

namespace CustomModel
{
    public class PlayerPrefsData
    {
        /// <summary>
        /// 对应内网版,外网版等各个版本
        /// </summary>
        public string mVersion;

        /// <summary>
        /// 对应内网版,外网版等各个版本
        /// </summary>
        public string Version
        {
            get
            {
                return mVersion;
            }
            set
            {
                if(mVersion!=value)
                {
                    mVersion = value;
                    Save();
                }
            }
        }
        /// <summary>
        /// 上一次进入游戏的版本
        /// </summary>
        public string mLastAppVersion;
        /// <summary>
        /// 上一次进入游戏的版本
        /// </summary>
        public string LastAppVersion
        {
            get
            {
                return mLastAppVersion;
            }
            set
            {
                if (mLastAppVersion != value)
                {
                    mLastAppVersion = value;
                    Save();
                }
            }
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        public bool mCheckUpdate;
        /// <summary>
        /// 检查更新
        /// </summary>
        public bool CheckUpdate
        {
            get
            {
                return mCheckUpdate;
            }
            set
            {
                if (mCheckUpdate != value)
                {
                    mCheckUpdate = value;
                    Save();
                }
            }
        }

        /// <summary>
        /// 当没有保存的语言设置时,按系统语言选择语言.
        /// </summary>
        public SystemLanguage mSystemLanguage;
        public SystemLanguage SystemLanguage
        {
            get
            {
                return mSystemLanguage;
            }
            set
            {
                if(mSystemLanguage!=value)
                {
                    mSystemLanguage = value;
                    Save();
                    EventMgr.LanguageChange.Notify(mSystemLanguage);
                }
            }
        }

        /// <summary>
        /// 语言枚举名称
        /// </summary>
        public string LanguageName
        {
            get
            {
                return mSystemLanguage.ToString();
            }
        }

        /// <summary>
        /// Lua存储数据
        /// </summary>
        public string LuaData
        {
            get
            {
                return mLuaData;
            }
            set
            {
                if(mLuaData != value)
                {
                    mLuaData = value;
                    Save();
                }
            }
        }

        public string mLuaData;

        public void Save()
        {
            PlayerPrefs.SetString(SettingModel.dataKey, Torsion.Serialize(this));
            PlayerPrefs.Save();
        }
        public void Clear()
        {
            PlayerPrefs.DeleteKey(SettingModel.dataKey);
            PlayerPrefs.Save();
        }
    }
    public class SettingModel
    {
        static PlayerPrefsData ms_instance;
        public static PlayerPrefsData instance
        {
            get
            {
                if (ms_instance == null)
                {
                    if (!PlayerPrefs.HasKey(dataKey))
                    {
                        ms_instance = new PlayerPrefsData()
                        {
                            mSystemLanguage = Application.systemLanguage,
                            mCheckUpdate = true,
                        };
                        instance.Save();
                    }
                    else
                    {
                        ms_instance = Torsion.Deserialize<PlayerPrefsData>(PlayerPrefs.GetString(dataKey));
                    }
                }
                return ms_instance;
            }
        }

        public static string dataKey
        {
            get
            {
                return Application.identifier+ ApplicationUtil.runByMobileDevice;
            }
        }

        

        //public void SetKey<T>(string key,T v)
        //{
        //    switch(typeof(T).Name)
        //    {
        //        case "System.String":
        //            {
        //                PlayerPrefs.SetString(key, System.Convert.ToString(v));
        //                break;
        //            }
        //        case "System.Int32":
        //            {
        //                PlayerPrefs.SetInt(key, System.Convert.ToInt32(v));
        //                break;
        //            }
        //        case "System.Single":
        //            {
        //                PlayerPrefs.SetFloat(key, System.Convert.ToInt32(v));
        //                break;
        //            }
        //    }
        //}

        //public T GetKey<T>(string key)
        //{
        //    switch (typeof(T).Name)
        //    {
        //        case "System.String":
        //            {
        //                return ConvertUtil.ChangeType<T>(PlayerPrefs.GetString(key));
        //            }
        //        case "System.Int32":
        //            {
        //                return ConvertUtil.ChangeType<T>(PlayerPrefs.GetInt(key));
        //            }
        //        case "System.Single":
        //            {
        //                return ConvertUtil.ChangeType<T>(PlayerPrefs.GetFloat(key));
        //            }
        //    }
        //    return default(T);
        //}

        //public void DeleteKey(string key)
        //{
        //    PlayerPrefs.DeleteKey(key );
        //}
        //public bool HasKey(string key)
        //{
        //    return PlayerPrefs.HasKey(key);
        //}
        //public void DeleteAll()
        //{
        //    PlayerPrefs.DeleteAll();
        //}
        //public void Save()
        //{
        //    PlayerPrefs.Save();
        //}
    }
}
