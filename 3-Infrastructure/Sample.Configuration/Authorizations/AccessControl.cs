using Sample.Commons.Enums;
using System.Reflection;

namespace Sample.Configuration.Authorizations
{
    public class AccessTheControllers
    {
        public AccessTheControllers()
        {
            Controllers = new List<ControllerData>();
        }

        public List<ControllerData> Controllers { get; set; }

        public Task FillListControllers(Assembly assembly, Type baseController)
        {
            var listControllers = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(baseController)).ToList();

            foreach (var controller in listControllers)
            {
                var data = new ControllerData();

                data.ControllerName = controller.Name.Replace("Controller", "");

                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

                foreach (var method in methods)
                {
                    var action = new ControllerActionData();

                    action.ActionName = method.Name;

                    var userAccountRolesAttribute = method.GetCustomAttributes().
                        FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(UserRolesAttribute)));

                    if (userAccountRolesAttribute != null)
                    {
                        action.UserRoles = (userAccountRolesAttribute as UserRolesAttribute).AcceptableRoles;
                    }

                    data.Actions.Add(action);
                }

                Controllers.Add(data);
            }

            return Task.CompletedTask;
        }
    }

    public class ControllerData
    {
        public ControllerData()
        {
            Actions = new List<ControllerActionData>();
        }

        public string ControllerName { get; set; }

        public List<ControllerActionData> Actions { get; set; }
    }

    public class ControllerActionData
    {
        public string ActionName { get; set; }
        public UserRole[] UserRoles { get; set; }
    }
}
