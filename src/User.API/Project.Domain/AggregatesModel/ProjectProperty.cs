using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty:ValueObject
    {

        public ProjectProperty()
        {

        }

        public ProjectProperty(string key,string text,string value):this()
        {
            Key = key;
            Text = text;
            Value = value;
        }
        public int ProjectId { get; set; }

        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Text;
            yield return Value;
        }
    }
}
