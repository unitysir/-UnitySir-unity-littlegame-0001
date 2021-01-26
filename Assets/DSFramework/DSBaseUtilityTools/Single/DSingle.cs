using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSFramework {

    public class DSingle<T> where T : new() {
        private static T instance;
        private static object obj = new object();

        public static T DSInstance {
            get {
                if (instance == null) {
                    lock (obj) {
                        if (instance == null) {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }

}