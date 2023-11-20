using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EncryptStringSample;
using Newtonsoft.Json;
using Zoka.CSCommon;
using Zoka.Paddle.ConfigurationManager.Abstraction;

namespace Zoka.Paddle.ConfigurationManager.File
{

	/// <summary>Notifies listeners than queue has been changed</summary>
	public delegate void ConfigurationChangeEventHandler(int _num_keys);

	/// <inheritdoc />
	public class FileConfigurationManager : IConfigurationManager
	{
		class DataItem
		{
			public string									Key { get; }
			public object									Value { get; }
			public bool										Secured { get; }
			public Type										Type { get; }

			public DataItem(string _key, object _value, bool _secured, Type _type)
			{
				Key = _key;
				Value = _value;
				Secured = _secured;
				Type = _type;
			}
		}

		//private readonly Dictionary<string, object>			m_Configs = new Dictionary<string, object>();
		//private readonly Dictionary<string, object>			m_SecuredConfigs = new Dictionary<string, object>();
		private readonly List<DataItem>						m_Data = new List<DataItem>();
		private readonly string								m_ConfigurationFilename;
		private const string								PASS = "ijkXh65%or<aRTsc";
		/// <summary>Event which allows clients to monitor changes </summary>
		public event ConfigurationChangeEventHandler		OnConfigurationChanged;


		/// <summary>Constructor</summary>
		public FileConfigurationManager(string _configuration_filename)
		{
			m_ConfigurationFilename = _configuration_filename;
		}

		/// <summary>
		/// Event which allows clients to monitor changes
		/// </summary>
		/// <param name="_queue_size"></param>
		public void RaiseOnConfigurationChanged(int _queue_size)
		{
			OnConfigurationChanged?.Invoke(_queue_size);
		}

		/// <summary>Will retrieve the configuration by its id or null, when not found</summary>
		/// <exception cref="InvalidCastException">The stored configuration is not of type</exception>
		/// <remarks>Returned instance of configuration is deep clone of the stored information, so it is safe to change internal values. Changes are not reflected in the storage</remarks>
		public T											Get<T>(string _id, T _default_value = default(T))
		{
			lock (m_Data)
			{
				var item = m_Data.SingleOrDefault(d => d.Key == _id);
				if (item == null)
					return _default_value;

				if (!typeof(T).IsAssignableFrom(item.Type))
					throw new InvalidCastException($"Data requested as {typeof(T).FullName}, but inner data is of type {item.Type.FullName}");

				return (T)item.Value;
			}
		}

		/// <summary>Will retrieve the configurations with their ids by their ids, if they match the passed key regex AND if the type is T</summary>
		/// <exception cref="InvalidCastException">Some of the matching configuration is not of type T</exception>
		public IEnumerable<Tuple<string, T>>				GetByRegex<T>(string _regex)
		{
			lock (m_Data)
			{
				var regex = new Regex(_regex);
				var items = m_Data.Where(d => regex.IsMatch(d.Key) && typeof(T).IsAssignableFrom(d.Type));

				var datas = items.Select(di => new Tuple<string, T>(di.Key, (T)di.Value)).ToList();
				return datas;
			}
		}

		/// <summary>Will retrieve the configuration with their ids if the configuration is of T type (or assignable to T)</summary>
		public IEnumerable<Tuple<string, T>>				GetByType<T>()
		{
			lock (m_Data)
			{
				var items = m_Data.Where(d => typeof(T).IsAssignableFrom(d.Type));

				var datas = items.Select(di => new Tuple<string, T>(di.Key, (T)di.Value)).ToList();
				return datas;
			}
		}

		/// <summary>Will return the keys matching with the passed regex.</summary>
		/// <remarks>
		///		The function returns list of keys, which are present at the evaluation moment. After finishing the function, the data may be changed
		///		so your code should be protected against possibility the key has been already removed.
		/// </remarks>
		public IEnumerable<string>							GetKeys(string _regex_filter = ".*")
		{
			lock (m_Data)
			{
				var regex = new Regex(_regex_filter);
				var items = m_Data.Where(d => regex.IsMatch(d.Key));

				var keys = items.Select(di => di.Key).ToList();
				return keys;
			}
		}


		/// <summary>Will store the configuration by its id</summary>
		/// <exception cref="ArgumentException">When there exists the configuration with the same id</exception>
		public Task											Add<T>(string _id, T _configuration, bool _is_secured = false, bool _force_immediate_persistance = false)
		{
			lock (m_Data)
			{
				var item = m_Data.SingleOrDefault(d => d.Key == _id);
				if (item != null)
					throw new ArgumentException($"Key {_id} already exists in configuration");

				m_Data.Add(new DataItem(_id, _configuration, _is_secured, _configuration.GetType()));
			}

			return StoreConfiguration(_force_immediate_persistance);
		}

		/// <summary>Will store, or update existing configuration by its id</summary>
		public Task											AddOrUpdate<T>(string _id, T _configuration, bool _is_secured = false, bool _force_immediate_persistance = false)
		{
			lock (m_Data)
			{
				var item_idx = m_Data.FindIndex(d => d.Key == _id);
				if (item_idx == -1)
					m_Data.Add(new DataItem(_id, _configuration, _is_secured, _configuration.GetType()));
				else
				{
					m_Data[item_idx] = new DataItem(_id, _configuration, _is_secured, _configuration.GetType());
				}
			}

			return StoreConfiguration(_force_immediate_persistance);
		}

		/// <summary>Will update the configuration of some id</summary>
		/// <exception cref="ArgumentException">When there do not exists the configuration with the same id</exception>
		public Task											Update<T>(string _id, Func<T, T> _update_configuration_action, bool _is_secured, bool _force_immediate_persistance = false)
		{
			lock (m_Data)
			{
				var item_idx = m_Data.FindIndex(d => d.Key == _id);
				if (item_idx == -1)
					throw new ArgumentException($"Configuration with key {_id} not exists.");

				if (!typeof(T).IsAssignableFrom(m_Data[item_idx].Type))
					throw new InvalidCastException($"Data requested as {typeof(T).FullName}, but inner data is of type {m_Data[item_idx].Type.FullName}");

				var updated_obj = _update_configuration_action((T)m_Data[item_idx].Value);
				if (!Object.ReferenceEquals(updated_obj, m_Data[item_idx].Value))
					m_Data[item_idx] = new DataItem(_id, updated_obj, _is_secured, updated_obj.GetType());
			}

			return StoreConfiguration(_force_immediate_persistance);
		}

		/// <summary>Will remove the configurations from the storage by their ids</summary>
		public Task											Remove(IEnumerable<string> _ids, bool _force_immediate_persistance = false)
		{
			int removed = 0;

			lock (m_Data)
			{
				foreach (var id in _ids)
				{
					var item_idx = m_Data.FindIndex(d => d.Key == id);
					if (item_idx != -1)
					{
						removed++;
						m_Data.RemoveAt(item_idx);
					}
				}
			}

			if (removed > 0)
				return StoreConfiguration(_force_immediate_persistance);

			return Task.CompletedTask;
		}

		/// <summary>Will load and overwrite existing configuration (if any) from the file</summary>
		public Task											Load()
		{
			lock(m_Data)
				m_Data.Clear();

			var file = new FileInfo(m_ConfigurationFilename);
			if (file.Exists)
			{
				using (var fr = file.OpenText())
				using (var jr = new JsonTextReader(fr))
				{
					var jser = JsonSerializer.Create();
					var tuples = jser.Deserialize<List<(string, bool, string, string)>>(jr);

					foreach (var conf_item in tuples)
					{
						string obj_json = conf_item.Item4;
						if (conf_item.Item2)
							obj_json = StringCipher.Decrypt(obj_json, PASS);
						var tgt_type = Type.GetType(conf_item.Item3);
						if (tgt_type == null)
							throw new Exception($"Error Loading configuration. Expected type definition not found {conf_item.Item3}");

						var obj = JsonConvert.DeserializeObject(obj_json, tgt_type, GetJsonSerializerSettings());
						
						if (obj == null)
							throw new Exception($"Unable to deserialize json configuation into type {tgt_type}");

						lock (m_Data)
						{
							m_Data.Add(new DataItem(conf_item.Item1, obj, conf_item.Item2, tgt_type));
						}
					}

				}
			}

			return Task.CompletedTask;
		}

		/// <summary>
		///		When not overriden, it stores configuration in the file immediately not reflecting the _force_immediate_persistance parameter value.
		///		You should override this method, when you want to support delayed/buffered/background storing of configuration in the file. 
		/// </summary>
		protected virtual Task								StoreConfiguration(bool _force_immediate_persistance)
		{
			return StoreConfigurationInFile();
		}

		/// <summary>Stores the current state of configuration into the file.</summary>
		protected Task										StoreConfigurationInFile()
		{
			var file_tuples = new List<(string, bool, string, string)>();

			lock (m_Data)
			{
				foreach (var data_item in m_Data)
				{
					var obj_serialized = JsonConvert.SerializeObject(data_item.Value, GetJsonSerializerSettings());
					if (data_item.Secured)
						obj_serialized = StringCipher.Encrypt(obj_serialized, PASS);
					file_tuples.Add((data_item.Key, data_item.Secured, data_item.Type.AssemblyQualifiedName, obj_serialized));
				}
			}

			var file = new FileInfo(m_ConfigurationFilename);
			using (var fs = file.Open(FileMode.Create, FileAccess.Write))
			using (var jw = new StreamWriter(fs))
			{
				var jser = JsonSerializer.Create();
				jser.Serialize(jw, file_tuples);
			}
			RaiseOnConfigurationChanged(m_Data.Count);
			return Task.CompletedTask;
		}

		/// <summary>Will retrieve JsonSerializedSettings</summary>
		protected virtual JsonSerializerSettings			GetJsonSerializerSettings()
		{
			var sett = new JsonSerializerSettings();
			sett.DateParseHandling = DateParseHandling.None;
			sett.Culture = CultureInfo.InvariantCulture;
			return sett;
		}

	}
}
