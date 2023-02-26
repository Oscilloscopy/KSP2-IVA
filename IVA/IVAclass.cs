
using KSP.DebugTools;
using KSP.Sim.impl;
using SpaceWarp.API;
using UnityEngine;
using KSP.Game;
using System;
using System.Runtime.CompilerServices;
using KSP.VFX;
using System.Collections.Generic;
using System.Linq;
using KSP.Logging;

namespace Autostrut
{

    public class IVAclass : Mod
    {

        Camera cam1 = null;
        Camera cam2 = null;
        float zoom;
        float xrot = 0.001f;
        float yrot = 0.625f;
        bool IVA = false;
        Transform origparent;
        Vector3 origloc = Vector3.zero;
        Vector3 origrot = Vector3.zero;
        float origfov;
        
        void Update()
        {
            //Change IVA on C keypress
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Find the 2 cameras
                Camera[] cameras = (Camera[])GameObject.FindObjectsOfType(typeof(Camera));
                foreach (Camera c in cameras)
                {

                    if (c.name == "FlightCameraPhysics_Main")
                    {
                        cam1 = c;

                    }
                    if (c.name == "kerbalCam")
                    {
                        cam2 = c;

                    }

                }
                if (IVA)
                {
                    
                    //Getting out of iva
                    cam1.transform.SetParent(origparent);
                    cam1.transform.localPosition = origloc;
                    cam1.transform.localEulerAngles = origrot;
                    cam1.fieldOfView = origfov;
                    
                }
                else
                {
                    //Getting into iva
                    origparent = cam1.transform.parent;
                    origloc = cam1.transform.localPosition;
                    origrot = cam1.transform.localEulerAngles;
                    origfov = cam1.fieldOfView;
                    cam1.transform.SetParent(cam2.transform.parent);
                }

                //Do some camera yoga
                cam1.transform.position = cam2.transform.position;
                cam1.transform.rotation = cam2.transform.rotation;
                cam1.transform.localPosition = new Vector3(-0.001f, 0.625f, 0.1f);
                cam1.transform.localEulerAngles = new Vector3(-0.001f, 0.625f, 0.208f);

                cam1.nearClipPlane = 0.02f;
                IVA = !IVA;
                //Hide or show the kerbal body (Set active did some weird stuff)
                if (IVA)
                {
                    cam2.transform.parent.FindChildRecursive("bone_kerbal_master_parent").parent.localScale = new Vector3(0, 0, 0);
                } else
                {
                    cam2.transform.parent.FindChildRecursive("bone_kerbal_master_parent").parent.localScale = new Vector3(1, 1, 1);
                }
            }
            if (IVA)
            {
                float mx = Input.GetAxis("Mouse X");
                float my = Input.GetAxis("Mouse Y");

                zoom += Input.mouseScrollDelta.y * Time.deltaTime * 200;
                zoom = Mathf.Clamp(zoom, 30, 120);
                xrot += mx * Time.deltaTime * 30;
                yrot -= my * Time.deltaTime * 30;
                cam1.fieldOfView = zoom;
                cam1.transform.localEulerAngles = new Vector3(yrot,xrot, 0.208f);

                

                
            }
        }

    }
}

