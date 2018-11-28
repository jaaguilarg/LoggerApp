using LoggerApp.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoggerApp.Utility
{
    public class HelperGeneric
    {
        public static T IntToEnum<T>(int id)
        {
            T status = (T)Enum.Parse(typeof(T), id.ToString());
            return status;
        }

        /// <summary>
        /// Convierte un objeto Tipo DataTable a una lista generica tipo T
        /// </summary>
        /// <typeparam name="T">Tipo de entidad</typeparam>
        /// <param name="dt">Objeto DataTable a convertir</param>
        /// <returns>Retorna una lista tipo T</returns>
        public static List<T> MapDataTableToList<T>(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count < 0)
                {
                    return new List<T>();
                }

                List<T> result = new List<T>();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr != null)
                    {
                        result.Add(MapDataRowToObject<T>(dr));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Convierte un objeto tipo DataRow a una entidad de negocio tipo T
        /// </summary>
        /// <typeparam name="T">Tipo al cual se convertira el objeto DataRow</typeparam>
        /// <param name="dr">Objeto DataRow a convertir</param>
        /// <returns></returns>
        public static T MapDataRowToObject<T>(DataRow dr)
        {
            string propiedadActual = string.Empty;
            try
            {
                if (dr == null)
                    return default(T);
                T instance = Activator.CreateInstance<T>();
                PropertyInfo[] properties = instance.GetType().GetProperties();
                if ((properties.Length > 0))
                {
                    foreach (PropertyInfo propertyObject in properties)
                    {
                        if (!propertyObject.CanWrite)
                            continue;

                        Reflexion attr = propertyObject.GetCustomAttributes(typeof(Reflexion), false).Cast<Reflexion>().FirstOrDefault();
                        if (attr != null && !attr.Readable)
                            continue;

                        if (dr.Table.Columns.Contains(propertyObject.Name) && (!object.ReferenceEquals(dr[propertyObject.Name], DBNull.Value)))
                        {
                            propiedadActual = propertyObject.Name;
                            if (propertyObject.PropertyType.Name == "Decimal")
                                propertyObject.SetValue(instance, Convert.ToDecimal(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "Double")
                                propertyObject.SetValue(instance, Convert.ToDouble(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "Int32")
                                propertyObject.SetValue(instance, Convert.ToInt32(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "Int64")
                                propertyObject.SetValue(instance, Convert.ToInt64(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "Single")
                                propertyObject.SetValue(instance, Convert.ToSingle(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "DateTime")
                                propertyObject.SetValue(instance, Convert.ToDateTime(dr[propertyObject.Name]), null);
                            else if (propertyObject.PropertyType.Name == "String")
                                propertyObject.SetValue(instance, Convert.ToString(dr[propertyObject.Name]), null);
                            else
                                propertyObject.SetValue(instance, dr[propertyObject.Name], null);
                        }
                        else if (propertyObject.PropertyType.Name == "String")
                            propertyObject.SetValue(instance, "", null);
                        else if (propertyObject.PropertyType.Name == "Single")
                            propertyObject.SetValue(instance, Convert.ToSingle(0), null);
                        else if (propertyObject.PropertyType.Name == "Decimal")
                            propertyObject.SetValue(instance, Convert.ToDecimal(0), null);
                        else if (propertyObject.PropertyType.Name == "Integer")
                            propertyObject.SetValue(instance, 0, null);
                    }
                }
                return instance;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al mapear la propiedad '" + propiedadActual + "'", ex);
            }
        }

        /// <summary>
        /// convierte un objeto a un Dictionary
        /// </summary>
        /// <param name="Obj">objeto a convertir</param>
        /// <returns></returns>
        public static Dictionary<string, string> MapObjectToDictionary(object Obj, Func<string, string> func = null)
        {
            Dictionary<string, string> ObjDict = new Dictionary<string, string>();
            PropertyInfo[] properties = Obj.GetType().GetProperties();
            foreach (PropertyInfo propertyObject in properties)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj))))
                    {
                        SqlParametros attr = propertyObject.GetCustomAttributes(typeof(object), false).Cast<SqlParametros>().FirstOrDefault();
                        if (attr == null || (attr != null && attr.IsParameter))
                        {
                            string valor = string.Empty;



                            if (Obj.GetType().GetProperty(propertyObject.Name).PropertyType.Name == "Nullable`1")
                            {
                                Type originalType = Type.GetType(Obj.GetType().GetProperty(propertyObject.Name).PropertyType.AssemblyQualifiedName);
                                Type underlyingType = Nullable.GetUnderlyingType(originalType);
                                if (underlyingType.Name == "DateTime")
                                {
                                    if ((DateTime)Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj) != default(DateTime))
                                        valor = Convert.ToString(Convert.ToDateTime(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj)).ToString("dd/MM/yyyy HH:mm"));
                                }
                                else
                                if (underlyingType.Name == "Decimal")
                                {
                                    valor = Convert.ToString(Convert.ToInt64(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj)));
                                }
                                else
                                if (underlyingType.Name == "Single")
                                {
                                    valor = Convert.ToString(Convert.ToSingle(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj))).Replace(",", ".");
                                }
                                else
                                {
                                    valor = Convert.ToString(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj));
                                }

                            }
                            else
                            {
                                if (Obj.GetType().GetProperty(propertyObject.Name).PropertyType.Name == "DateTime")
                                {
                                    if ((DateTime)Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj) != default(DateTime))
                                        valor = Convert.ToString(Convert.ToDateTime(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj)).ToString("dd/MM/yyyy HH:mm"));
                                }
                                else
                                if (Obj.GetType().GetProperty(propertyObject.Name).PropertyType.Name == "Decimal")
                                {

                                    valor = Convert.ToString(Convert.ToInt64(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj)));
                                }
                                else
                                if (Obj.GetType().GetProperty(propertyObject.Name).PropertyType.Name == "Single")
                                {

                                    valor = Convert.ToString(Convert.ToSingle(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj))).Replace(",", ".");
                                }
                                else
                                    valor = Convert.ToString(Obj.GetType().GetProperty(propertyObject.Name).GetValue(Obj));
                            }
                            if (string.IsNullOrEmpty(valor))
                                valor = string.Empty;
                            else
                                if (func != null)
                                valor = func(valor);
                            ObjDict.Add(propertyObject.Name, valor);
                        }
                    }
                    else
                    {
                        SqlParametros attr = propertyObject.GetCustomAttributes(typeof(object), false).Cast<SqlParametros>().FirstOrDefault();
                        if (attr == null || (attr != null && attr.IsParameter))
                            ObjDict.Add(propertyObject.Name, "");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ObjDict;
        }

        public static T DictionaryToObject<T>(Dictionary<string, object> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }
    }
}
