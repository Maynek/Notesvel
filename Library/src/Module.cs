//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Collections.Generic;

namespace Maynek.Notesvel.Library
{
    public class ModuleTable : ElementTableBase<Module>
    {
        //================================
        // Methods
        //================================
        public void Build()
        {
            var rootModules = new List<Module>();

            foreach (var m in this)
            {
                var isRoot = true;
                if (m.HasBase)
                {
                    if (this.Contains(m.BaseId))
                    {
                        isRoot = false;
                        var pm = this[m.BaseId];
                        m.AddWaitingModule(pm);
                        pm.AddFollower(m);
                    }
                    else
                    {
                        throw new NotesvelException(m.Id + ":BaseId(" + m.BaseId + ") is unknown.");
                    }
                }

                if (isRoot)
                {
                    rootModules.Add(m);
                }
            }

            foreach (var rm in rootModules)
            {
                rm.Build(this);
            }

            return;
        }
    }
        

    public abstract class Module : Element
    {
        //================================
        // Properties
        //================================
        public string BaseId { get; set; } = string.Empty;
        public bool HasBase { get { return (this.BaseId != string.Empty); } }

        public string OriginalValue { get; set; } = string.Empty;
        public string BuiltValue { get; protected set; } = string.Empty;

        private ModuleTable WaitingModules   { get; } = new ModuleTable();
        private ModuleTable FollowerModules { get; } = new ModuleTable();
        private bool IsWaiting { get { return (this.WaitingModules.Count > 0); } }


        //================================
        // Override Methods
        //================================
        public override void AppendInformation(InformationBuilder builder)
        {
            base.AppendInformation(builder);
            builder.AppendIndentedLine("BaseId=" + this.BaseId);
        }


        //================================
        // Methods
        //================================
        protected abstract string BuildValue(ModuleTable moduleTable);

        public string GetValue()
        {
            return this.BuiltValue;
        }

        public void AddWaitingModule(Module module)
        {
            this.WaitingModules.Add(module);
        }

        public void AddFollower(Module follower)
        {
            this.FollowerModules.Add(follower);
        }

        private void NotifyBuildFinished(Module module)
        {
            this.WaitingModules.Remove(module.Id);
        }

        public void Build(ModuleTable moduleTable)
        {
            if (this.IsWaiting)
            {
                return;
            }

            Logger.D("Build Module. ModuleName=" + this.Name);

            //Read original value.
            this.OriginalValue = this.ReadValue();

            //Build Value.
            this.BuiltValue = this.BuildValue(moduleTable);

            //Build follwers.
            foreach (var fm in this.FollowerModules)
            {
                fm.NotifyBuildFinished(this);
                fm.Build(moduleTable);
            }
        }
    }
}
