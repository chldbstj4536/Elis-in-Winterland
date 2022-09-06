using System;
using System.Collections.Generic;

public class DataContext
{
	public event Action<string> contextChanged = delegate { };
	private IDictionary<string, object> m_ActiveBinds = new Dictionary<string, object>();

	public bool ContainsKey(string key)
	{
		return m_ActiveBinds.ContainsKey(key);
	}

	public object this[string key] {
		get {
			object val = ContainsKey(key) ? m_ActiveBinds[key] : null;
			//UnityEngine.Debug.Log($"BindTarget TRY? : {ContainsKey(key)}  KEY : {key}  :: VALUE : {val}");
			return m_ActiveBinds[key];
		}
		set {
			if (value == null) {
				return;
			}

			m_ActiveBinds[key] = value;

			if (value is INotifyCollectionChanged) {
				((INotifyCollectionChanged)value).collectionChanged += contextChanged;
			}

			contextChanged(key);
		}
	}
}
