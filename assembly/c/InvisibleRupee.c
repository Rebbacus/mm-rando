#include <z64.h>
#include "Item00.h"
#include "Player.h"

void InvisibleRupee_GiveItem(Actor* actor, GlobalContext* ctxt) {
    u16 type = actor->params & 0x03;
    ActorEnItem00* item = z2_fixed_drop_spawn(ctxt, &actor->currPosRot.pos, type);

    u16 flag = actor->params >> 2;
    u16 giIndex = 0x350 + flag;
    Item00_CheckAndSetGiIndex(item, ctxt, giIndex);
}