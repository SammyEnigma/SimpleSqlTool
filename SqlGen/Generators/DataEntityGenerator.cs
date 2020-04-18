﻿using SqlGen.Templeates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class DataEntityGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {
            
            var dataEntity = new EntityTempleates();   
            dataEntity.Session = new Dictionary<string, object>();
            dataEntity.Session.Add("_namespace", "PenMail");
            dataEntity.Session.Add("table", table);
            dataEntity.Session.Add("tableName", table.TableName.ToPascalCase());
            dataEntity.Session.Add("tableNameToLower", table.TableName);

            var fk = table.ForeignKeys.ToForegnTableColumns();

            dataEntity.Session.Add("foregnkeys", fk);

            var columns = table.Columns.Where(c => !c.IsRowVersion() && (options.Audit || !c.IsAuditColumn()));
            dataEntity.Session.Add("columns", columns);
            dataEntity.Initialize();
            

            return dataEntity.TransformText();
        }



        public override string ToString() => "Data Entity";
    }
}
