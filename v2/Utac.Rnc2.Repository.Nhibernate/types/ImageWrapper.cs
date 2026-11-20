using System;
using System.Data;
using System.Data.Common;
using System.IO;
using NHibernate;
using NHibernate.Engine;
using NHibernate.UserTypes;
using Utac.Rnc2.Infrastructure.types;

namespace Utac.Rnc2.Repository.Nhibernate.types
{
    public class ImageWrapper : IUserType
    {

        public NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get { return new NHibernate.SqlTypes.SqlType[] { NHibernateUtil.BinaryBlob.SqlType }; }
        }

        public Type ReturnedType
        {
            get { return typeof(Image); }
        }

        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            int length = 32768; // 20 Mb
            byte[] buf = new byte[length];
            long dataIndex = 0;
            long bytesRead = 0;
            MemoryStream ms = new MemoryStream();
            do
            {
                bytesRead = rs.GetBytes(rs.GetOrdinal(names[0]), dataIndex, buf, 0, length);
                dataIndex += length;
                ms.Write(buf, 0, (int)bytesRead);
                buf = new byte[length];
            }
            while (bytesRead == length);
            ms.Position = 0;
            var image = new Image(ms.GetBuffer()); //create from bytes
            ms.Close();
            ms.Dispose();
            return image;

        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (IDataParameter)cmd.Parameters[index];
            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = ((Image)value).InternalBytes;

        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }
    }
}
