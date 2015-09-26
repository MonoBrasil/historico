using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Reflection;
using PHP.Core;


namespace PHP {


	public abstract class Mixed {
		public int __placeOnHeap;
		public Mixed() {
			__placeOnHeap = 0;
		}
	}

	public class Boolean : Mixed {
		public bool value;
		public Boolean(bool value) : base() {
			this.value = value;
		}
		public override bool Equals(object o) {
			if (o is Boolean)
				return value.Equals(((Boolean)o).value);
			else if (o is Mixed)
				return Equals(Core.Convert.ToBoolean((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return value.GetHashCode();
		}
		public override string ToString() {
			return value.ToString(); ;
		}
	}


	public class Integer : Mixed {
		public int value;
		public Integer(int value) : base() {
			this.value = value;
		}
		public override bool Equals(object o) {
			if (o is Integer)
				return value.Equals(((Integer)o).value);
			else if (o is Mixed)
				return Equals(Core.Convert.ToInteger((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return value.GetHashCode();
		}
		public override string ToString() {
			return value.ToString(); ;
		}
	}


	public class Double : Mixed {
		public double value;
		public Double(double value) : base() {
			this.value = value;
		}
		public override bool Equals(object o) {
			if (o is Double)
				return value.Equals(((Double)o).value);
			else if (o is Mixed)
				return Equals(Core.Convert.ToDouble((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return value.GetHashCode();
		}
		public override string ToString() {
			return value.ToString(); ;
		}
	}


	public class String : Mixed {
		public string value;
		public String(string value) : base() {
			this.value = value;
		}
		public override bool Equals(object o) {
			if (o is String)
				return value.Equals(((String)o).value);
			else if (o is Mixed)
				return Equals(Core.Convert.ToString((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return value.GetHashCode();
		}
		public override string ToString() {
			return value.ToString(); ;
		}
	}


	public class Array : Mixed {
		public ArrayList keys;
		public ArrayList values;
		public int maxKey;
		public int current;
		public Array() : this(new ArrayList(), new ArrayList()) {}
		public Array(ArrayList keys, ArrayList values) : base() {
			this.keys = keys;
			this.values = values;
			// calculate maximum key
			maxKey = -1;
			foreach (Mixed key in keys) {
				if (key is Integer) {
					int keyValue = ((Integer)key).value;
					if (keyValue > maxKey)
						maxKey = keyValue;
				}
			}
			current = 0;
		}
		public void Append(Mixed value) {
			Append(null, value);
		}
		public void Append(Mixed key, Mixed value) {
			// if value isn't on heap yet, determine new place on heap and store it there
			if (value.__placeOnHeap == 0) {
				value.__placeOnHeap = ++Runtime.maxPlaceOnHeap;
				Runtime.heap[value.__placeOnHeap] = value;
			}
			// append value to array
			if (key == null) {
				keys.Add(new Integer(++maxKey));
				values.Add(value.__placeOnHeap);
			}
			else if (key is Boolean) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				Append(intKey, value);
			}
			else if (key is Integer) {
				int index = keys.IndexOf(key);
				if (index > -1) {
					keys[index] = key;
					values[index] = value.__placeOnHeap;
				}
				else {
					keys.Add(key);
					values.Add(value.__placeOnHeap);
				}
				int keyValue = ((Integer)key).value;
				if (keyValue > maxKey)
					maxKey = keyValue;
			}
			else if (key is Double) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				Append(intKey, value);
			}
			else if (key is String) {
				string keyValue = ((String)key).value;
				if (IsStandardInteger(keyValue)) {
					int intKeyValue = System.Convert.ToInt32(keyValue);
					Integer intKey = new Integer(intKeyValue);
					Append(intKey, value);
				}
				else {
					int index = keys.IndexOf(key);
					if (index > -1) {
						keys[index] = key;
						values[index] = value.__placeOnHeap;
					}
					else {
						keys.Add(key);
						values.Add(value.__placeOnHeap);
					}
				}
			}
			else if (key is Null) {
				String stringKey = PHP.Core.Convert.ToString(key);
				Append(stringKey, value);
			}
			// other data types are not allowed for keys, so ignore them
			else
				Report.Warn(402);
		}
		public void Remove(Mixed key) {
			if (key == null)
				return;
			else if (key is Boolean) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				Remove(intKey);
			}
			else if (key is Integer) {
				int index = keys.IndexOf(key);
				if (index > -1) {
					keys.RemoveAt(index);
					values.RemoveAt(index);
				}
			}
			else if (key is Double) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				Remove(intKey);
			}
			else if (key is String) {
				string keyValue = ((String)key).value;
				if (IsStandardInteger(keyValue)) {
					int intKeyValue = System.Convert.ToInt32(keyValue);
					Integer intKey = new Integer(intKeyValue);
					Remove(intKey);
				}
				else {
					int index = keys.IndexOf(key);
					if (index > -1) {
						keys.RemoveAt(index);
						values.RemoveAt(index);
					}
				}
			}
			else if (key is Null) {
				String stringKey = PHP.Core.Convert.ToString(key);
				Remove(stringKey);
			}
			// other data types are not allowed for keys, so ignore them
			else
				Report.Warn(402);
		}
		public Mixed Get(Mixed key) {
			if (key is Boolean) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				return Get(intKey);
			}
			else if (key is Integer) {
				int index = keys.IndexOf(key);
				if (index > -1) {
					int placeOnHeap = (int)values[index];
					return Runtime.LoadFromHeap(placeOnHeap);
				}
				else
					return new Null();
			}
			else if (key is Double) {
				Integer intKey = PHP.Core.Convert.ToInteger(key);
				return Get(intKey);
			}
			else if (key is String) {
				string keyValue = ((String)key).value;
				if (IsStandardInteger(keyValue)) {
					int intKeyValue = System.Convert.ToInt32(keyValue);
					Integer intKey = new Integer(intKeyValue);
					return Get(intKey);
				}
				else {
					int index = keys.IndexOf(key);
					if (index > -1) {
						int placeOnHeap = (int)values[index];
						return Runtime.LoadFromHeap(placeOnHeap);
					}
					else
						return new Null();
				}
			}
			else if (key is Null) {
				String stringKey = PHP.Core.Convert.ToString(key);
				return Get(stringKey);
			}
			// other data types are not allowed for keys, so ignore them
			else {
				Report.Warn(402);
				return new Null();
			}
		}
		public Mixed Key() {
			if (Core.Lang.CurrentIsValid(this))
				return (Mixed)keys[current];
			else
				return new Boolean(false);
		}
		public Mixed Current() {
			if (Core.Lang.CurrentIsValid(this)) {
				int placeOnHeap = (int)values[current];
				return Runtime.LoadFromHeap(placeOnHeap);
			}
			else
				return new Boolean(false);
		}
		public Mixed Next() {
			current++;
			return Current();
		}
		public Mixed Prev() {
			current--;
			return Current();
		}
		public Mixed Reset() {
			current = 0;
			return Current();
		}
		public static bool IsStandardInteger(string s) {
			if (s.Length == 0)
				return false;
			if (s == "0")
				return true;
			if (s[0] < '1' || s[0] > '9')
				return false;
			for (int i = 1; i < s.Length; i++)
				if (s[0] < '0' || s[0] > '9')
					return false;
			return true;
		}
		public override bool Equals(object o) {
			if (o is Array) {
				Array a = (Array)o;
				if (keys.Count != a.keys.Count)
					return false;
				for (int i = 0; i < keys.Count; i++) {
					Mixed key = (Mixed)keys[i];
					int value = (int)values[i];
					int i2 = a.keys.IndexOf(key);
					if (i2 == -1)
						return false;
					if (!key.Equals(a.keys[i2]))
						return false;
					if (value != (int)a.values[i2])
						return false;
				}
				return true;
			}
			else if (o is Mixed)
				return Equals(Core.Convert.ToArray((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			long result = 0;
			foreach (Mixed o in keys)
				result += o.GetHashCode();
			foreach (int i in values)
				result += i.GetHashCode();
			return (int)result;
		}
		public override string ToString() {
			return "Array";
		}
	}


	public abstract class Object : Mixed {
		public static int __maxId = 0;
		public int __id;
		public Object() : base() {
			__id = ++__maxId;
		}
		public override bool Equals(object o) {
			if (o is Object) {
				Array result = new Array();
				// first check if both Objects have the same amount of fields
				if (this.GetType().GetFields().Length != o.GetType().GetFields().Length)
					return false;
				// then check equality of fields
				foreach (FieldInfo f in this.GetType().GetFields()) {
					// don't use the internal fields __id and __maxId
					if (f.Name != "__id" && f.Name != "__maxId") {
						// check if o has the same field
						if (o.GetType().GetField(f.Name) == null)
							return false;
						// if so, check if values are the same
						if (!f.GetValue(this).Equals(f.GetValue(o)))
							return false;
					}
				}
				return true;
			}
			else if (o is Mixed)
				return Equals(Core.Convert.ToObject((Mixed)o));
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return __id;
		}
	}

	public class stdClass : Object {
		public Mixed scalar;
		public stdClass(Mixed scalar) : base() {
			this.scalar = scalar;
		}
	}

	public class Null : Mixed {
		public Null() : base() { }
		public override bool Equals(object o) {
			if (o is Null)
				return true;
			else if (o is Mixed)
				return false;
			else
				return base.Equals(o);
		}
		public override int GetHashCode() {
			return 0;
		}
		public override string ToString() {
			return "Null"; ;
		}
	}

}