using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleInterface
{
    public struct User
    {
        public object this[string property]
        {
            get
            {
                switch (property)
                {
                    case "name":
                        return name;
                    case "id":
                        return id;
                    default:
                        throw new ArgumentException("property doesn't exist");
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Cannot set value to null");
                }
                switch (property)
                {
                    case "name":
                        name = (string)value;
                        break;
                    case "id":
                        id = (string)value;
                        break;
                    default:
                        throw new ArgumentException("property doesn't exist");
                }
            }
        } 
        public string name;
        public string id;

        public static bool operator ==(User a, User b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(User a, User b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            User other;
            try
            {
                other = (User)obj;
            }
            catch (Exception)
            {
                return base.Equals(obj);
            }

            return name == other.name && id == other.id;
        }
        public override int GetHashCode()
        {
            return name.GetHashCode() * id.GetHashCode();
        }
    }
    public struct FacebookMessage
    {
        public int id;
        public string message;
        public User sender;
        public DateTime timeSent;

        public object this[string property]
        {
            get
            {
                switch (property)
                {
                    case "message":
                        return message;
                    case "id":
                        return id;
                    case "sender":
                        return sender;
                    case "timeSent":
                        return timeSent;
                    default:
                        throw new ArgumentException("property doesn't exist");
                }
            }
            set
            {
                switch (property)
                {
                    case "message":
                        if (((object)value).GetType() == typeof(System.DBNull))
                        {
                            message = null;
                        }
                        else
                        {
                            message = (string)value;
                        }

                        
                        break;
                    case "id":
                        id = (int)value;
                        break;
                    case "sender":
                        sender = (User)value;
                        break;
                    case "timeSent":
                        timeSent = (DateTime)value;
                        break;
                    default:
                        throw new ArgumentException("property doesn't exist");
                }
            }
        }

        public static bool operator ==(FacebookMessage a, FacebookMessage b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(FacebookMessage a, FacebookMessage b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
            FacebookMessage other;

            try
            {
                other = (FacebookMessage)obj;
            }
            catch (Exception)
            {
                return base.Equals(obj);
            }

            return id == other.id;
        }
        public override int GetHashCode()
        {
            return id;
        }
    }
}
