using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureStorage
{
    public class TableAlreadyExists : Exception
    {
        public TableAlreadyExists() : base("Table already exists") { }
    }
    public class TableNotFound : Exception
    {
        public TableNotFound() : base("Table not found") { }
    }
    public class TableBeingDeleted : Exception
    {
        public TableBeingDeleted() : base("Table is being deleted") { }
    }
    public class EntityAlreadyExists : Exception
    {
        public EntityAlreadyExists() : base("Entity already exists") { }
    }
}