using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleInterface
{
    public struct User
    {
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
        public string message;
        public User sender;
        public DateTime timeSent;

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

            return sender == other.sender && timeSent.Equals(other.timeSent) && message == other.message;
        }
        public override int GetHashCode()
        {
            return message.GetHashCode() * sender.GetHashCode() * timeSent.GetHashCode();
        }
    }
}
