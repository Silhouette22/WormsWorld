using System;
using System.Text.Json.Serialization;

namespace ActionProviderLib
{
    public class ActionDto
    {
        public ActionType Type { get; set; }

        [JsonConstructor]
        public ActionDto(ActionType type)
        {
            Type = type;
        }

        public ActionDto(Action action)
        {
            if (action == Actions.MoveUp)
                Type = ActionType.MoveUp;
            else if (action == Actions.MoveDown)
                Type = ActionType.MoveDown;
            else if (action == Actions.MoveRight)
                Type = ActionType.MoveRight;
            else if (action == Actions.MoveLeft)
                Type = ActionType.MoveLeft;
            else if (action == Actions.MultiplyUp)
                Type = ActionType.MultiplyUp;
            else if (action == Actions.MultiplyDown)
                Type = ActionType.MultiplyDown;
            else if (action == Actions.MultiplyRight)
                Type = ActionType.MultiplyRight;
            else if (action == Actions.MultiplyLeft)
                Type = ActionType.MultiplyLeft;
            else if (action == Actions.DoNothing)
                Type = ActionType.DoNothing;
            else
                throw new ArgumentOutOfRangeException(nameof(action));
        }
        public Action ToAction()
        {
            return Type switch
            {
                ActionType.MoveUp => Actions.MoveUp,
                ActionType.MoveDown => Actions.MoveDown,
                ActionType.MoveRight => Actions.MoveRight,
                ActionType.MoveLeft => Actions.MoveLeft,
                ActionType.MultiplyUp => Actions.MultiplyUp,
                ActionType.MultiplyDown => Actions.MultiplyDown,
                ActionType.MultiplyRight => Actions.MultiplyRight,
                ActionType.MultiplyLeft => Actions.MultiplyLeft,
                ActionType.DoNothing => Actions.DoNothing,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    



    public enum ActionType
    {
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        MultiplyUp,
        MultiplyDown,
        MultiplyRight,
        MultiplyLeft,
        DoNothing
    }
}