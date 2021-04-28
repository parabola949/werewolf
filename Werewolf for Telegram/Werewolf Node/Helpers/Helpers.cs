using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Werewolf_Node.Helpers
{
    internal static class Helpers
    {
        internal static void KickChatMember(long chatid, long userid)
        {
            var status = Program.Bot.GetChatMemberAsync(chatid, (int)userid).Result.Status; // APIV5 CAST

            if (status == ChatMemberStatus.Administrator) //ignore admins
                return;
            //kick
            Program.Bot.KickChatMemberAsync(chatid, (int)userid); // APIV5 CAST
            //get their status
            status =Program.Bot.GetChatMemberAsync(chatid, (int)userid).Result.Status; // APIV5 CAST
            while (status == ChatMemberStatus.Member) //loop
            {
                //wait for database to report status is kicked.
                status =Program.Bot.GetChatMemberAsync(chatid, (int)userid).Result.Status; // APIV5 CAST
                Thread.Sleep(100);
            }
            //status is now kicked (as it should be)

            while (status != ChatMemberStatus.Left) //unban until status is left
            {
               Program.Bot.UnbanChatMemberAsync(chatid, (int)userid); // APIV5 CAST
                Thread.Sleep(100);
                status =Program.Bot.GetChatMemberAsync(chatid, (int)userid).Result.Status; // APIV5 CAST
            }
            //yay unbanned

        }
    }
}
