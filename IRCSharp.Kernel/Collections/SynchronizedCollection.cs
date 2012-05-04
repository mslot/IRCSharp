using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Collections
{
	/// <summary>
	/// A synchronized dictionary class.
	/// Uses ReaderWriterLockSlim to handle locking. The dictionary does not allow recursion by enumeration. It is purly used for quick read access. 
	/// Maybe an enumerator will be made in the future.
	/// </summary>
	/// <typeparam name="T">Type that is going to be kept.</typeparam>
	public sealed class SynchronizedDictionary<U, T> : IEnumerable<KeyValuePair<U, T>>
	{
		private System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();
		private IDictionary<U, T> _collection = null;

		public SynchronizedDictionary()
		{
			_collection = new Dictionary<U, T>();
		}

		/// <summary>
		/// if getting:
		/// Enters read lock.
		/// Tries to get the value.
		/// 
		/// if setting:
		/// Enters write lock.
		/// Tries to set value.
		/// </summary>
		/// <param name="key">The key to fetch the value with.</param>
		/// <returns>Object of T</returns>
		public T this[U key]
		{
			get
			{
				_lock.EnterReadLock();
				try
				{
					return _collection[key];
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}

			set
			{
				Add(key, value);
			}

		}

		public bool ContainsKey(U key)
		{
			_lock.EnterReadLock();
			try
			{
				return _collection.ContainsKey(key);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}

		/// <summary>
		/// Enters write lock. 
		/// Removes key from collection
		/// </summary>
		/// <param name="key">Key to remove.</param>
		public void Remove(U key)
		{
			_lock.EnterWriteLock();
			try
			{
				_collection.Remove(key);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Enters write lock.
		/// Adds value to the collection if key does not exists.
		/// </summary>
		/// <param name="key">Key to add.</param>
		/// <param name="value">Value to add.</param>
		private void Add(U key, T value)
		{
			_lock.EnterWriteLock();
			try
			{
				_collection[key] = value;
			}
			finally
			{
				_lock.ExitWriteLock();
			}

		}

		public T TryGet(U key)
		{
			T value = default(T);
			_lock.EnterReadLock();
			try
			{
				if (_collection.ContainsKey(key))
				{
					value = _collection[key];
				}
			}
			finally
			{
				_lock.ExitWriteLock();
			}

			return value;
		}

		public void TryInsert(U key, T value)
		{
			_lock.EnterWriteLock();
			try
			{
				if (!_collection.ContainsKey(key))
				{
					_collection[key] = value;
				}
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Creates an enumerator.
		/// </summary>
		/// <returns>Returns an enumerator for a copy of the internal dictionary</returns>
		IEnumerator<KeyValuePair<U, T>> IEnumerable<KeyValuePair<U, T>>.GetEnumerator()
		{
			Dictionary<U, T> dictionary = GetUnsynchronizedDictionary();
			return dictionary.GetEnumerator();
		}

		/// <summary>
		/// Creates an enumerator.
		/// </summary>
		/// <returns>Returns an enumerator for a copy of the internal dictionary</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			Dictionary<U, T> dictionary = GetUnsynchronizedDictionary();
			return dictionary.GetEnumerator();
		}

		public Dictionary<U, T> GetUnsynchronizedDictionary()
		{
			_lock.EnterReadLock();
			try
			{
				return new Dictionary<U, T>(_collection);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}
	}
}
