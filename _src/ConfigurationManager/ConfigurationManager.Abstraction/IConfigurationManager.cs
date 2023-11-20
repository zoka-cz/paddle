using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoka.CSCommon;

namespace Zoka.Paddle.ConfigurationManager.Abstraction
{
	/// <summary>Interface defining method available for configuration manager</summary>
	public interface IConfigurationManager
	{
		/// <summary>Will retrieve the configuration by its id or null, when not found</summary>
		/// <exception cref="InvalidCastException">The stored configuration is not of type</exception>
		/// <remarks>Returned instance of configuration is deep clone of the stored information, so it is safe to change internal values. Changes are not reflected in the storage</remarks>
		T Get<T>(string _id, T _default_value = default(T));

		/// <summary>Will retrieve the configurations with their ids by their ids, if they match the passed key regex AND if the type is T</summary>
		/// <remarks>If the configuration is not of T type, but key matches the _regex, it will not be part of returned data</remarks>
		IEnumerable<Tuple<string, T>> GetByRegex<T>(string _regex);

		/// <summary>Will retrieve the configuration with their ids if the configuration is of T type (or assignable to T)</summary>
		IEnumerable<Tuple<string, T>> GetByType<T>();

		/// <summary>Will return the keys matching with the passed regex.</summary>
		/// <remarks>
		///		The function returns list of keys, which are present at the evaluation moment. After finishing the function, the data may be changed
		///		so your code should be protected against possibility the key has been already removed.
		/// </remarks>
		IEnumerable<string> GetKeys(string _regex_filter = ".*");

		/// <summary>Will store the configuration by its id</summary>
		/// <exception cref="ArgumentException">When there exists the configuration with the same id</exception>
		Task Add<T>(string _id, T _configuration, bool _is_secured = false, bool _force_immediate_persistance = false);

		/// <summary>Will store, or update existing configuration by its id</summary>
		Task AddOrUpdate<T>(string _id, T _configuration, bool _is_secured = false, bool _force_immediate_persistance = false);
		
		/// <summary>Will update the configuration of some id</summary>
		/// <exception cref="ArgumentException">When there do not exists the configuration with the same id</exception>
		Task Update<T>(string _id, Func<T, T> _update_configuration_action, bool _is_secured, bool _force_immediate_persistance = false);

		/// <summary>Will remove the configurations from the storage by their ids</summary>
		Task Remove(IEnumerable<string> _ids, bool _force_immediate_persistance = false);

		/// <summary>Will load and overwrite existing configuration (if any) from the file</summary>
		Task Load();

	}
}
