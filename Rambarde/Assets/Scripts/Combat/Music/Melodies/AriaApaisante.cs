using System.Linq;
using System.Threading.Tasks;
using Characters;
using Melodies;
using Status;
using UnityEngine;

namespace Music.Melodies {
    [CreateAssetMenu(fileName = "AriaApaisante", menuName = "Melody/AriaApaisante")]
    class AriaApaisante : Melody {
        protected override async Task ExecuteOnTarget(CharacterControl t)
        {
            await CombatManager.Instance.dialogManager.ShowDialog(DialogFilter.CombatStart, CharacterType.Bard,
                CharacterType.None, CombatManager.Instance.bard.instruments[1].sprite, "Theodore");
            await t.Heal(10);
                            
            await CombatManager.Instance.dialogManager.ShowDialog(DialogFilter.Heal,
                CharacterType.None, Dialog.GetCharacterTypeFromCharacterControl(t),
                t.characterData.clientImage, t.characterName);
            
            
            if (t.HasEffect(EffectType.Poison)) {
                await t.statusEffects.First(e => e.type == EffectType.Poison).RemoveEffect();
            }
            if (t.HasEffect(EffectType.Confused)) {
                await t.statusEffects.First(e => e.type == EffectType.Confused).RemoveEffect();
            }
            if (t.HasEffect(EffectType.Destabilized)) {
                await t.statusEffects.First(e => e.type == EffectType.Destabilized).RemoveEffect();
            }
        }
    }
}