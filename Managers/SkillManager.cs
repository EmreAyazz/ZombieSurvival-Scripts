using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Player Skill")]
    public List<GameObject> skills;
    public List<float> skillDamages;

    [Header("BOSS 1")]
    public GameObject kaya;
    public GameObject kayaParcali;
    public GameObject yereVurma;

    [Header("BOSS 2")]
    public GameObject tukuruk;
    public GameObject tukurukBefore;
    public GameObject tukurukParticle;
    public GameObject kusmaParticle;
    public GameObject kusmaYerParticle;

    [Header("BOSS 3")]
    public GameObject carpmaParticle;
    public GameObject ziplamaIndicator;

    [Header("BOSS 4")]
    public GameObject fuzeIndicator;
    public GameObject fuzeParticle;
    public GameObject fuzeObject;

    [Header("BOSS 5")]
    public GameObject areaArrowIndicator;
    public GameObject arrow;
    public GameObject arrowParticle;

    [Header("BOSS 6")]
    public GameObject gurbuz;
    public GameObject donmeIndicator;

    [Header("BOSS 7")]
    public GameObject testere;
    public GameObject testereIndicator;

    [Header("BOSS 8")]
    public GameObject fireBall;
    public GameObject flameThowerIndicator;

    [Header("BOSS 9")]
    public GameObject simsek;
    public GameObject enerjialan;
    public GameObject gucToplama;
    public GameObject elektrikAlanIndicator;

    [Header("BOSS 10")]
    public GameObject bombIndicator;
    public GameObject bombParticle;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Kaya(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newKaya = Instantiate(kaya, self.transform.position, Quaternion.identity);
        newKaya.SetActive(true);

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newKaya);
                break;
            }

            newKaya.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
            newKaya.transform.LookAt(player.transform.position);

            self.anim.SetBool("Skill1", false);

            if (Vector3.Distance(newKaya.transform.position, player.transform.position) <= 1f)
            {
                yakala = true;
                GameObject newPatlama = Instantiate(kayaParcali, newKaya.transform.position, newKaya.transform.rotation);
                newPatlama.SetActive(true);

                if (self.boss)
                    player.health -= self.skills[0].damage;
                else
                    player.health -= self.skills[0].damage * 0.1f;

                Destroy(newKaya);
                for (int i = 0; i < newPatlama.transform.childCount; i++)
                {
                    if (newPatlama.transform.GetChild(i).GetComponent<Rigidbody>())
                        newPatlama.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(500f, newPatlama.transform.position, 50f, 0);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator YereVurma(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newYereVurma = Instantiate(yereVurma, (self.transform.position + (self.transform.forward * 1f)), Quaternion.identity);
        newYereVurma.SetActive(true);
        newYereVurma.transform.LookAt((self.transform.position + (self.transform.forward * 10)));

        bool vurma = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float x = -1;
        float indikator = 30;
        yield return new WaitForSeconds(0.001f);

        self.agent.isStopped = true;

        while (!vurma)
        {
            if (!self)
            {
                Destroy(newYereVurma);
                break;
            }

            time += Time.deltaTime;

            if (x < 1)
                x += Time.deltaTime * 3;
            else
                x = -1;

            if (indikator > 0)
                indikator -= Time.deltaTime * 12;

            newYereVurma.transform.GetChild(0).GetComponent<Renderer>().material.SetVector("Vector2_c2e2b980180c44b38fdc06c40e9fb6fb", new Vector4(x, 0, 0, 0));
            newYereVurma.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("Vector1_1b13bbc3c23848b88f0edc16171a6303", indikator);

            if (time >= animTime)
            {
                vurma = true;
                self.anim.SetBool("Skill2", false);
                self.agent.isStopped = false;

                GameObject newParticle = Instantiate(self.hitParticle, self.hitParticle.transform.position, self.hitParticle.transform.rotation);
                newParticle.GetComponent<ParticleSystem>().Play();

                Destroy(newYereVurma);
                self.inSkill = false;

                if (player.inSkillArea == newYereVurma)
                {
                    if (self.boss)
                        player.health -= self.skills[1].damage;
                    else
                        player.health -= self.skills[1].damage * 0.1f;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Tukuruk(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newTukurukBefore = Instantiate(tukurukBefore, self.mouthArea.transform.position, Quaternion.identity);
        newTukurukBefore.SetActive(true);

        GameObject newTukuruk = Instantiate(tukuruk, self.mouthArea.transform.position, Quaternion.identity);
        newTukuruk.SetActive(true);

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newTukuruk);
                Destroy(newTukurukBefore);
                break;
            }

            newTukuruk.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
            newTukuruk.transform.LookAt(player.transform.position);

            self.anim.SetBool("Skill1", false);

            if (Vector3.Distance(newTukuruk.transform.position, player.transform.position) <= 1f)
            {
                yakala = true;
                GameObject newPatlama = Instantiate(tukurukParticle, newTukuruk.transform.position, newTukuruk.transform.rotation);
                newPatlama.SetActive(true);

                if (self.boss)
                    player.health -= self.skills[0].damage;
                else
                    player.health -= self.skills[0].damage * 0.1f;

                Destroy(newTukuruk);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Kusma(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newKusmaBefore = Instantiate(kusmaParticle, self.mouthArea.transform.position, Quaternion.identity, self.mouthArea.transform);
        newKusmaBefore.SetActive(true);
        newKusmaBefore.transform.LookAt(self.mouthArea.transform.position + (self.mouthArea.transform.forward * 5));

        GameObject newKusma = Instantiate(kusmaYerParticle, player.transform.position, Quaternion.identity);
        newKusma.SetActive(true);

        bool kusma = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float indicator = 6.5f;
        yield return new WaitForSeconds(0.001f);

        newKusmaBefore.GetComponent<ParticleSystem>().Play();
        self.agent.isStopped = true;

        while (!kusma)
        {
            if (!self)
            {
                Destroy(newKusmaBefore);
                Destroy(newKusma);
                break;
            }

            time += Time.deltaTime;

            indicator -= Time.deltaTime * 2.6f;

            newKusma.GetComponent<Renderer>().material.SetFloat("Vector1_429c5e62e86044eb9692e913d65b925d", indicator);

            if (time >= animTime)
            {
                kusma = true;
                self.anim.SetBool("Skill2", false);
                newKusmaBefore.GetComponent<ParticleSystem>().Stop();
                newKusma.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                newKusma.GetComponent<MeshFilter>().mesh = null;
                Destroy(newKusmaBefore);
                self.agent.isStopped = false;

                DestroyForWaiting wait = newKusma.gameObject.AddComponent<DestroyForWaiting>();
                wait.maxTime = 5f;

                SkillArea skill = newKusma.gameObject.AddComponent<SkillArea>();
                skill.damage = self.skills[1].damage * 0.1f;
                skill.hasarCoolDown = 0.1f;

                self.inSkill = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Chargelama(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        float lastSpeed = self.agent.speed;
        float lastAcceleration = self.agent.acceleration;
        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (self.myArea != player.area)
            {
                self.agent.isStopped = true;
                self.agent.speed = lastSpeed;
                self.agent.acceleration = lastAcceleration;
                self.anim.SetBool("Skill1", false);
                yakala = true;
                break;
            }

            self.agent.speed = player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 5;
            self.agent.acceleration = 80;
            self.agent.SetDestination(player.transform.position);

            if (Vector3.Distance(self.transform.position, player.transform.position) <= 3f)
            {
                self.agent.isStopped = true;
                self.agent.speed = lastSpeed;
                self.agent.acceleration = lastAcceleration;
                yakala = true;
                GameObject newPatlama = Instantiate(carpmaParticle, player.transform.position, player.transform.rotation);
                newPatlama.SetActive(true);

                player.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
                player.transform.parent.GetComponent<JoystickPlayerExample>().Invoke("isKinematicEnable", 0.5f);
                player.transform.parent.GetComponent<Rigidbody>().AddForce(self.transform.forward * 2500f);

                if (self.boss)
                    player.health -= self.skills[0].damage;
                else
                    player.health -= self.skills[0].damage * 0.1f;

                self.anim.SetBool("Skill1", false);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Ziplama(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        Vector3 pos = player.transform.position;

        float lastSpeed = self.agent.speed;
        float lastAcceleration = self.agent.acceleration;

        GameObject newZiplama = Instantiate(ziplamaIndicator, player.transform.position, Quaternion.identity);
        newZiplama.SetActive(true);

        bool ziplama = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float indicator = 6.5f;
        yield return new WaitForSeconds(0.001f);
        self.agent.isStopped = true;

        while (!ziplama)
        {
            if (!self)
            {
                Destroy(newZiplama);
                break;
            }

            time += Time.deltaTime;

            if (time > 2f)
            {
                self.agent.isStopped = false;
                self.agent.SetDestination(pos);
                self.agent.speed = Vector3.Distance(pos, self.transform.position) / 0.4f;
                self.agent.acceleration = 150;
            }

            indicator -= Time.deltaTime * 2.6f;

            newZiplama.GetComponent<Renderer>().material.SetFloat("Vector1_429c5e62e86044eb9692e913d65b925d", indicator);

            if (time >= animTime)
            {
                ziplama = true;
                self.anim.SetBool("Skill2", false);
                Destroy(newZiplama);
                GameObject newPatlama = Instantiate(carpmaParticle, pos, Quaternion.identity);
                newPatlama.SetActive(true);

                self.agent.speed = lastSpeed;
                self.agent.acceleration = lastAcceleration;

                self.inSkill = false;

                if (player.inSkillArea == newZiplama)
                {
                    if (self.boss)
                        player.health -= self.skills[1].damage;
                    else
                        player.health -= self.skills[1].damage * 0.1f;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Fuze(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newFuze = Instantiate(fuzeObject, self.mouthArea.transform.position, Quaternion.identity);
        newFuze.SetActive(true);

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newFuze);
                break;
            }

            newFuze.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
            newFuze.transform.LookAt(player.transform.position);

            self.anim.SetBool("Skill1", false);

            if (Vector3.Distance(newFuze.transform.position, player.transform.position) <= 1f)
            {
                yakala = true;
                GameObject newPatlama = Instantiate(fuzeParticle, newFuze.transform.position, newFuze.transform.rotation);
                newPatlama.SetActive(true);

                if (self.boss)
                    player.health -= self.skills[0].damage;
                else
                    player.health -= self.skills[0].damage * 0.1f;

                Destroy(newFuze);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator YukariFuze(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        bool fuze = false;
        float time = 0;
        float animTime = 2f;
        bool lastFuze = false;
        self.inSkill = true;
        float indicator = 6.5f;
        yield return new WaitForSeconds(0.001f);

        List<GameObject> fuzeAreas = new List<GameObject>();

        self.agent.isStopped = true;

        for (int i = 0; i < 6; i++)
        {
            GameObject newArea = Instantiate(fuzeIndicator, self.GetRandomPoint(self.transform, 10f), Quaternion.identity);
            newArea.transform.eulerAngles = new Vector3(-90, 0, 0);
            newArea.SetActive(true);
            float sure = Random.Range(1.5f, 3f);
            newArea.transform.GetChild(0).DOMoveY(0, sure).SetEase(Ease.Linear).OnComplete(() => 
            {
                GameObject newParticle = Instantiate(fuzeParticle, newArea.transform.position, Quaternion.identity);
                newParticle.SetActive(true);

                if (player.inSkillArea == newArea)
                {
                    if (self.boss)
                        player.health -= self.skills[1].damage;
                    else
                        player.health -= self.skills[1].damage * 0.1f;
                }

                Destroy(newArea);

            }).OnUpdate(() =>
            {
                indicator = 6.5f * (Vector3.Distance(newArea.transform.GetChild(0).position, newArea.transform.position) / 75f);
                newArea.GetComponent<Renderer>().material.SetFloat("Vector1_429c5e62e86044eb9692e913d65b925d", indicator);
            });
            fuzeAreas.Add(newArea);
        }

        while (!fuze)
        {
            if (!self)
            {
                for (int i = 5; i >= 0; i++)
                {
                    Destroy(fuzeAreas[i].gameObject);
                    fuzeAreas.RemoveAt(i);
                }
                break;
            }

            time += Time.deltaTime;

            if (time >= 0.5f && !lastFuze)
            {
                GameObject newMainFuze = Instantiate(fuzeObject, self.mouthArea.transform.position, Quaternion.identity);
                newMainFuze.SetActive(true);
                newMainFuze.transform.LookAt(self.transform.position + (self.transform.up * 50));
                newMainFuze.transform.DOMoveY(100, 1);
                lastFuze = true;
            }

            if (time >= animTime)
            {
                fuze = true;

                self.anim.SetBool("Skill2", false);
                self.agent.isStopped = false;
                self.inSkill = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Arrow(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        float animTime = 1f;
        float time = 0;

        self.arrow.SetActive(true);
        GameObject newArrow = Instantiate(arrow, self.mouthArea.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newArrow);
                break;
            }

            time += Time.deltaTime;

            if (time >= animTime)
            {
                newArrow.SetActive(true);
                newArrow.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
                newArrow.transform.LookAt(player.transform.position);

                self.anim.SetBool("Skill1", false);
                self.arrow.SetActive(false);

                if (Vector3.Distance(newArrow.transform.position, player.transform.position) <= 1f)
                {
                    yakala = true;
                    GameObject newPatlama = Instantiate(arrowParticle, newArrow.transform.position, newArrow.transform.rotation);
                    newPatlama.SetActive(true);

                    if (self.boss)
                        player.health -= self.skills[0].damage;
                    else
                        player.health -= self.skills[0].damage * 0.1f;

                    Destroy(newArrow);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator AreaArrow(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newArea = Instantiate(areaArrowIndicator, player.transform.position + new Vector3(0,0.1f,0), Quaternion.identity);
        newArea.transform.eulerAngles = new Vector3(-90, 0, 0);
        newArea.SetActive(true);

        bool atma = false;
        float time = 0;
        float animTime = 1f;
        float arrowTime = 0.5f;
        float totalArrowTime = 3f;
        float arrows = 0;
        bool lastArrow = false;
        self.inSkill = true;
        float indicator = 6.5f;
        self.agent.isStopped = true;

        yield return new WaitForSeconds(0.001f);

        while (!atma)
        {
            if (!self)
            {
                Destroy(newArea);
                break;
            }

            time += Time.deltaTime;

            indicator -= Time.deltaTime * 5;

            if (newArea && newArea.GetComponent<MeshFilter>().mesh)
                newArea.GetComponent<Renderer>().material.SetFloat("Vector1_429c5e62e86044eb9692e913d65b925d", indicator);

            if (time >= animTime)
            {
                if (!lastArrow)
                {
                    GameObject newMainArrow = Instantiate(arrow, self.mouthArea.transform.position, Quaternion.identity);
                    newMainArrow.SetActive(true);
                    newMainArrow.transform.LookAt(self.transform.position + (self.transform.up * 50));
                    newMainArrow.transform.DOMoveY(100, 1);
                    lastArrow = true;
                }

                self.anim.SetBool("Skill2", false);

                newArea.GetComponent<MeshFilter>().mesh = null;

                arrows += Time.deltaTime;

                if (arrows > arrowTime)
                {
                    GameObject newArrows = Instantiate(newArea.transform.GetChild(0).gameObject, newArea.transform.GetChild(0).position, newArea.transform.GetChild(0).rotation, newArea.transform);
                    newArrows.transform.eulerAngles = new Vector3(-90, 0, Random.Range(0f, 360f));
                    newArrows.SetActive(true);
                    arrows = 0;
                }

                self.agent.isStopped = false;

                DestroyForWaiting wait = newArea.gameObject.AddComponent<DestroyForWaiting>();
                wait.maxTime = 4f;

                SkillArea skill = newArea.gameObject.AddComponent<SkillArea>();

                if (self.boss)
                {
                    skill.damage = self.skills[1].damage * 0.1f;
                    skill.hasarCoolDown = 0.1f;
                }
                else
                {
                    skill.damage = self.skills[1].damage * 0.01f;
                    skill.hasarCoolDown = 0.1f;
                }

                self.inSkill = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator GurbuzFirlatma(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newGurbuz = Instantiate(gurbuz, self.mouthArea.transform.position, Quaternion.identity);
        newGurbuz.SetActive(true);

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newGurbuz);
                break;
            }

            newGurbuz.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
            newGurbuz.transform.LookAt(player.transform.position);
            newGurbuz.transform.GetChild(0).Rotate(newGurbuz.transform.GetChild(0).up * 360 * Time.deltaTime);

            self.anim.SetBool("Skill1", false);

            if (Vector3.Distance(newGurbuz.transform.position, player.transform.position) <= 1f)
            {
                yakala = true;
                GameObject newPatlama = Instantiate(fuzeParticle, newGurbuz.transform.position, newGurbuz.transform.rotation);
                newPatlama.SetActive(true);

                if (self.boss)
                    player.health -= self.skills[0].damage;
                else
                    player.health -= self.skills[0].damage * 0.1f;

                Destroy(newGurbuz);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator GurbuzDonme(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        Vector3 pos = player.transform.position;

        GameObject newDonme = Instantiate(donmeIndicator, self.transform.position, Quaternion.identity);
        newDonme.SetActive(true);

        bool donme = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float indicator = 0;
        yield return new WaitForSeconds(0.001f);
        self.agent.isStopped = true;

        while (!donme)
        {
            if (!self)
            {
                Destroy(newDonme);
                break;
            }

            time += Time.deltaTime;

            indicator += Time.deltaTime * 1f / animTime;

            newDonme.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Frac", indicator);

            self.arrow.GetComponent<SkillArea>().inSkill = true;

            if (time >= animTime)
            {
                donme = true;
                self.anim.SetBool("Skill2", false);
                Destroy(newDonme);

                self.inSkill = false;
                self.agent.isStopped = false;
                self.arrow.GetComponent<SkillArea>().inSkill = false;

                if (player.inSkillArea == newDonme)
                {
                    if (self.boss)
                        self.arrow.GetComponent<SkillArea>().damage = self.skills[1].damage;
                    else
                        self.arrow.GetComponent<SkillArea>().damage = self.skills[1].damage * 0.1f;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator TekliTestere(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();
        GameObject newTestere = null;

        float time = 0;
        float animTime = 1f;

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newTestere);
                break;
            }

            time += Time.deltaTime;

            if (time >= animTime)
            {
                self.anim.SetBool("Skill1", false);


                newTestere = Instantiate(testere, new Vector3(self.mouthArea.transform.position.x, 0, self.mouthArea.transform.position.z), Quaternion.identity);
                newTestere.SetActive(true);

                animTime = 10000;
            }

            if (newTestere)
            {
                newTestere.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
                newTestere.transform.LookAt(player.transform.position);
                if (Vector3.Distance(newTestere.transform.position, player.transform.position) <= 1f)
                {
                    yakala = true;
                    GameObject newPatlama = Instantiate(fuzeParticle, newTestere.transform.position, newTestere.transform.rotation);
                    newPatlama.SetActive(true);

                    if (self.boss)
                        player.health -= self.skills[0].damage;
                    else
                        player.health -= self.skills[0].damage * 0.1f;

                    Destroy(newTestere);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Testere(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newYereVurma = Instantiate(testereIndicator, (self.transform.position + (self.transform.forward * 1f)), Quaternion.identity);
        newYereVurma.SetActive(true);
        newYereVurma.transform.LookAt((self.transform.position + (self.transform.forward * 10)));

        bool vurma = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float x = -1;
        float indikator = 30;
        yield return new WaitForSeconds(0.001f);

        bool vurdu = false;

        self.agent.isStopped = true;

        while (!vurma)
        {
            if (!self)
            {
                Destroy(newYereVurma);
                break;
            }

            time += Time.deltaTime;

            if (x < 1)
                x += Time.deltaTime * 3;
            else
                x = -1;

            if (indikator > 0)
                indikator -= Time.deltaTime * 12;

            newYereVurma.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetVector("Vector2_c2e2b980180c44b38fdc06c40e9fb6fb", new Vector4(x, 0, 0, 0));
            newYereVurma.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetFloat("Vector1_1b13bbc3c23848b88f0edc16171a6303", indikator);
            newYereVurma.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetVector("Vector2_c2e2b980180c44b38fdc06c40e9fb6fb", new Vector4(x, 0, 0, 0));
            newYereVurma.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetFloat("Vector1_1b13bbc3c23848b88f0edc16171a6303", indikator);
            newYereVurma.transform.GetChild(2).GetChild(0).GetComponent<Renderer>().material.SetVector("Vector2_c2e2b980180c44b38fdc06c40e9fb6fb", new Vector4(x, 0, 0, 0));
            newYereVurma.transform.GetChild(2).GetChild(0).GetComponent<Renderer>().material.SetFloat("Vector1_1b13bbc3c23848b88f0edc16171a6303", indikator);

            if (time >= animTime)
            {
                for (int i = 0; i < newYereVurma.transform.childCount; i++)
                {
                    newYereVurma.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }

                if (player.inSkillArea == newYereVurma && !vurdu)
                {
                    if (self.boss)
                        player.health -= self.skills[1].damage;
                    else
                        player.health -= self.skills[1].damage * 0.1f;

                    vurdu = true;
                }

                if (time >= animTime + 1f)
                {
                    Destroy(newYereVurma);
                    self.anim.SetBool("Skill2", false);
                    self.agent.isStopped = false;
                    vurma = true;
                    self.inSkill = false;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator FireBall(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        bool yakala = false;
        Player player = GameObject.FindObjectOfType<Player>();
        GameObject newFireBall = null;

        float time = 0;
        float animTime = 0.5f;

        yield return new WaitForSeconds(0.001f);

        while (!yakala)
        {
            if (!self)
            {
                Destroy(newFireBall);
                break;
            }

            time += Time.deltaTime;

            if (time >= animTime)
            {
                self.anim.SetBool("Skill1", false);

                newFireBall = Instantiate(fireBall, self.mouthArea.transform.position, Quaternion.identity);
                newFireBall.SetActive(true);

                animTime = 10000;
            }

            if (newFireBall)
            {
                newFireBall.transform.Translate(Vector3.forward * Time.deltaTime * (player.transform.parent.GetComponent<JoystickPlayerExample>().speed + 1f));
                newFireBall.transform.LookAt(player.transform.position);
                if (Vector3.Distance(newFireBall.transform.position, player.transform.position) <= 1f)
                {
                    yakala = true;
                    GameObject newPatlama = Instantiate(fuzeParticle, newFireBall.transform.position, newFireBall.transform.rotation);
                    newPatlama.SetActive(true);

                    if (self.boss)
                        player.health -= self.skills[0].damage;
                    else
                        player.health -= self.skills[0].damage * 0.1f;

                    Destroy(newFireBall);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator FlameThower(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        Vector3 pos = player.transform.position;

        GameObject newDonme = Instantiate(flameThowerIndicator, self.transform.position - new Vector3(0, 1, 0), Quaternion.identity);
        newDonme.SetActive(true);

        bool donme = false;
        float time = 0;
        float animTime = 5f;
        self.inSkill = true;
        float indicator = 0;
        yield return new WaitForSeconds(0.001f);
        self.agent.isStopped = true;
        self.arrow.transform.GetChild(0).gameObject.SetActive(true);

        while (!donme)
        {
            if (!self)
            {
                Destroy(newDonme);
                break;
            }

            time += Time.deltaTime;

            indicator += Time.deltaTime * 1f / animTime;

            newDonme.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Frac", indicator);

            self.arrow.GetComponent<SkillArea>().inSkill = true;

            if (self.boss)
                self.arrow.GetComponent<SkillArea>().damage = self.skills[1].damage / 5f;
            else
                self.arrow.GetComponent<SkillArea>().damage = self.skills[1].damage * 0.1f / 5f;

            if (time >= animTime)
            {
                donme = true;

                self.inSkill = false;
                self.agent.isStopped = false;
                self.anim.SetBool("Skill2", false);
                self.arrow.GetComponent<SkillArea>().damage = 0;
                self.arrow.GetComponent<SkillArea>().inSkill = false;
                self.arrow.transform.GetChild(0).gameObject.SetActive(false);
                Destroy(newDonme);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Simsek(BossEnemy self)
    {
        self.anim.SetBool("Skill1", true);
        Player player = GameObject.FindObjectOfType<Player>();

        self.agent.isStopped = true;
        self.inSkill = true;

        yield return new WaitForSeconds(1.2f);

        GameObject newElektrik = Instantiate(simsek, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        newElektrik.SetActive(true);
        newElektrik.transform.eulerAngles = new Vector3(-90, 0, 0);

        yield return new WaitForSeconds(0.2f);

        if (self.boss)
            player.health -= self.skills[0].damage;
        else
            player.health -= self.skills[0].damage * 0.1f;

        self.anim.SetBool("Skill1", false);
        self.agent.isStopped = false;
        self.inSkill = false;
    }

    public IEnumerator ElektrikAlan(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        GameObject newYereVurma = Instantiate(elektrikAlanIndicator, (self.transform.position + (self.transform.forward * 1f)), Quaternion.identity);
        newYereVurma.SetActive(true);
        newYereVurma.transform.LookAt((self.transform.position + (self.transform.forward * 10)));

        bool vurma = false;
        float time = 0;
        float animTime = 2.5f;
        self.inSkill = true;
        float x = -1;
        float indikator = 60;
        yield return new WaitForSeconds(0.001f);

        self.agent.isStopped = true;
        self.arrow.SetActive(true);

        while (!vurma)
        {
            if (!self)
            {
                Destroy(newYereVurma);
                break;
            }

            time += Time.deltaTime;

            if (x < 1)
                x += Time.deltaTime * 3;
            else
                x = -1;

            if (indikator > 0)
                indikator -= Time.deltaTime * 24;

            newYereVurma.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("Vector1_4b2c1a05bee34f9294f0afe1cc304e9b", indikator);

            if (time >= animTime)
            {
                self.arrow.SetActive(false);
                GameObject newAlan = Instantiate(enerjialan, (self.transform.position + (self.transform.forward * 1f)), Quaternion.identity);
                newAlan.SetActive(true);
                newAlan.transform.LookAt((self.transform.position + (self.transform.forward * 10)));

                if (player.inSkillArea == newYereVurma)
                {
                    if (self.boss)
                        player.health -= self.skills[1].damage;
                    else
                        player.health -= self.skills[1].damage * 0.1f;
                }

                Destroy(newYereVurma);
                self.anim.SetBool("Skill2", false);
                self.agent.isStopped = false;
                vurma = true;
                self.inSkill = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Alev(BossEnemy self)
    {
        self.anim.SetBool("Skill2", true);
        Player player = GameObject.FindObjectOfType<Player>();

        bool fuze = false;
        float time = 0;
        float animTime = 2f;
        bool lastFuze = false;
        self.inSkill = true;
        float indicator = 6.5f;
        yield return new WaitForSeconds(0.001f);

        List<GameObject> fuzeAreas = new List<GameObject>();

        GameObject newBombArea = Instantiate(bombIndicator, (new Vector3(self.transform.position.x, 0.01f, self.transform.position.z)), Quaternion.identity);
        newBombArea.SetActive(true);
        newBombArea.transform.DOLookAt(self.transform.position + (self.transform.forward * 10), 0f, AxisConstraint.Y);

        self.agent.isStopped = true;

        float sure = 1f;
        int sayi = 0;
        int sayi2 = 0;
        for (int i = 0; i < newBombArea.transform.childCount; i++)
        {
            newBombArea.transform.GetChild(i).GetChild(0).DOMoveY(0, sure).SetEase(Ease.Linear).OnComplete(() =>
            {
                Debug.Log(sayi);
                newBombArea.transform.DOMoveY(0.01f, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log(sayi2);
                    GameObject newParticle = Instantiate(bombParticle, newBombArea.transform.GetChild(sayi2).position, Quaternion.identity);
                    newParticle.SetActive(true);

                    if (player.inSkillArea == newBombArea)
                    {
                        if (self.boss)
                            player.health -= self.skills[1].damage;
                        else
                            player.health -= self.skills[1].damage * 0.1f;
                    }

                    newBombArea.transform.GetChild(sayi2).gameObject.SetActive(false);

                    if (sayi2 == 5)
                    {
                        self.agent.isStopped = false;
                        self.inSkill = false;
                        self.anim.SetBool("Skill2", false);
                        Destroy(newBombArea);
                    }

                    sayi2++;
                    indicator = 6.5f;

                }).OnUpdate(() =>
                {
                    indicator -= Time.deltaTime * 6.5f;
                    newBombArea.transform.GetChild(sayi2).GetComponent<Renderer>().material.SetFloat("Vector1_429c5e62e86044eb9692e913d65b925d", indicator);
                });
                sayi++;

            });
            sure += 0.2f;
        }
    }
}
