using UnityEngine;
using System;
using System.Collections;

namespace DemoLand.PersistData
{
	public class PersistData : Singleton<PersistData>
	{
		private Hashtable data = new Hashtable();

		/// <summary>
		/// Set or update data
		/// </summary>
		/// <param name="key">Object's identifier</param>
		/// <param name="obj">The data</param>
		/// <returns></returns>
		public void Set(object key, object obj) {
			if (data.ContainsKey(key)) {
				data[key] = obj;
			} else {
				data.Add(key, obj);
			}
		}

		public object Get(object key) {
			if (data.ContainsKey(key)) {
				return data[key];
			}
			return null;
		}

		public void Del(object key) {
			if (data.ContainsKey(key)) {
				data.Remove(key);
			}
		}

		public bool Has(object key) {
			return data.ContainsKey(key);
		}
	}
}