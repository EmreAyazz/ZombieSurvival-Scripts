using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT.Weapon
{
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public int level;
        public float range;
        public float coolDown;
        public AudioClip audio;
        public Vector2 damage;
        public int weaponCount;
        public GameObject bullet;
        public GameObject muzzleEffect;
    }

    [System.Serializable]
    public class Skill
    {
        public enum skillName { Kaya, YereVurma, Tukuruk, Kusma, Ziplama, Chargelama, 
            Fuze, YukariFuze, Arrow, AreaArrow, GurbuzDonme, GurbuzFirlatma, 
            TekliTestere, Testere, FlameThower, FireBall, ElektrikAlan, Simsek, Alev };
        public skillName skill;
        public float damage;
        public float coolDown;
    }

    [System.Serializable]
    public class Companions
    {
        public GameObject companion;
        public GameObject area;
        public int level;
        public List<GameObject> openingObjects;
        public List<GameObject> closingObjects;
    }
}
