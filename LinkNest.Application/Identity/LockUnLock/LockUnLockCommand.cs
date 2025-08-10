
using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.LockUnLock
{
    public record LockUnLockCommand(string userId):ICommand<string>;
}
