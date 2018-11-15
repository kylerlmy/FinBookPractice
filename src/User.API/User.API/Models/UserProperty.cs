using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class UserProperty//属于用户的一部分，不会单独的去查询
    {
        private int? _requestedHashCode;

        public int AppUserId { get; set; }//使用默认约定的方式，与AppUser建立多对一的关系
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }


        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = (this.Key + this.Value).GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
            
        }

        public override bool Equals(object obj)
        {

            if (obj == null || !(obj is UserProperty))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            var item = (UserProperty)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Key == this.Key && item.Value == this.Value;
        }

        public bool IsTransient()
        {
            return string.IsNullOrEmpty(this.Key) || string.IsNullOrEmpty(this.Value);
        }

        public static bool operator ==(UserProperty left, UserProperty right)
        {
            if (object.Equals(left, null))
                return (object.Equals(right, null) ? true : false);
            else
                return left.Equals(right);
        }

        public static bool operator !=(UserProperty left, UserProperty right)
        {
            return !(left == right);
        }
    }
}
