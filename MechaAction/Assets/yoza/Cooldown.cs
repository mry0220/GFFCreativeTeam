using System;
using System.Collections;
using UnityEngine;


namespace Cooltime
{
    public class CoolDown
    {
        public delegate void Skill(int dir);



        public void CoolTime(float coolTime)
        { 
            float time = 0;

            while (time < coolTime)
            {
                time += Time.deltaTime;
            }
            return;
        }
        public IEnumerator SkillCooltime(float cooltime,Action skill = null,bool isCooltime = false)
        {
            skill();
                yield return new WaitForSeconds(cooltime);
        }

        public IEnumerator SkillDilayCooltime(float cooltime =0, Action skill = null,float countdown = 0)
        {
                yield return new WaitForSeconds(countdown);
                skill();
            yield return new WaitForSeconds(cooltime);
        }
    }
}

namespace Critical
{
    class CriticalDamage
    {
        public double damage(int ATK = 0,float critical = 5f, float criticalDamage = 50f)
        {
            float Crit = UnityEngine.Random.Range(1f, 100f);

            return (Crit < critical) ? ATK + (ATK *( criticalDamage * 0.01)) : ATK;

        }

    }

}


