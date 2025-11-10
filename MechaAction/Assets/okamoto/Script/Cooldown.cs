using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooltime
{
    public class CoolDown
    {
        public void CoolTime(float coolTime)
        { 
            float time = 0;

            while (time < coolTime)
            {
                time += Time.deltaTime;
            }
            return;
        }

        public IEnumerator SkillCooltime(float cooltime,bool IsCooltime = true)
        {
            if (IsCooltime == true)
            {
                yield return new WaitForSeconds(cooltime);
                IsCooltime = false;
            }
            else
                yield return null;
        }
    }
}


