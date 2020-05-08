﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientMenuManager : MonoBehaviour
{
    public Characters.CharacterData[] classList;
    [SerializeField]
    private int _selectedClient;
    public int SelectedClient
    {
        get { return _selectedClient; }
        set
        {
            _selectedClient = value;
            if (value >= 3)
                doneButton.GetComponent<Button>().interactable = true;
            else
                doneButton.GetComponent<Button>().interactable = false;
        }
    }

    ClientBehaviour[] clientList;
    Counter counter;
    protected GameObject doneButton;

    void Awake()
    {
        clientList = transform.GetChild(2).GetComponentsInChildren<ClientBehaviour>();
        counter = transform.GetComponentInChildren<Counter>();
        GenerateClients();
        doneButton = transform.GetChild(transform.childCount - 1).gameObject;
        doneButton.GetComponent<Button>().interactable = false;
    }

    public void resetSelectedClient(int nClient) { SelectedClient -= nClient; }

    public void resetClientMenu()
    {
        if (transform.GetComponentInParent<GuildeManagerBehaviour>().menuValid[0] && SelectedClient >= 3)
            return;

        counter.resetCounter();
        foreach (ClientBehaviour client in clientList)
            client.ResetSelected();
        transform.GetComponentInParent<GuildeManagerBehaviour>().resetClients();
    }

    public void selectClients()
    {
        List<Client> clients = new List<Client>();
        for (int i = 0; i < clientList.Length; i++)
        {
            if (!clientList[i].IsClickable)
            {
                clients.Add(clientList[i].client);
            }
        }

        transform.GetComponentInParent<GuildeManagerBehaviour>().SetClients(clients);
    }

    #region ClientsGeneration
    public void GenerateClients()
    {
        int[] classes = GenerateClasses();
        int[][] skills = GenerateSkillWheel();
        ClientNamesList clientNamesList = new ClientNamesList();
        clientNamesList.Init();

        for(int i = 0; i < clientList.Length; i++)
            clientList[i].client = new Client(classList[classes[i]], skills[i], clientNamesList.generateClientName());
    }

    private int[] GenerateClasses()
    {
        List<int> possibleClasses = new List<int>() { 0, 1, 2, 3, 4, 5 };
        List<int> classOccurence = new List<int>() { 0, 0, 0, 0, 0, 0 };
        int[] chosenClasses = new int[6];

        for (int i = 0; i < 6; i++) 
        {
            int n = Random.Range(0, possibleClasses.Count);
            classOccurence[n] += 1;
            chosenClasses[i] = possibleClasses[n];

            if (classOccurence[n] == 3)
            {
                classOccurence.RemoveAt(n);
                possibleClasses.RemoveAt(n);
            }
        }
        return chosenClasses;
    }
    private int[][] GenerateSkillWheel()
    {
        int[][] skillWheel = new int[6][];
        for (int i = 0; i < skillWheel.Length; i++)
            skillWheel[i] = new int[5];
        for(int i = 0; i < skillWheel.Length; i++)
        {
            int[] currentSW = skillWheel[i];
            List<int> tier1Choice = new List<int>() { 0, 1, 2, 3 };
            List<int> tier2Choice = new List<int>() { 4, 5 };
            List<int> tier3Choice = new List<int>() { 6, 7 };
            
            for (int j = 0; j < currentSW.Length - 3; j++)
            {
                int n = Random.Range(0, tier1Choice.Count);
                currentSW[j] = tier1Choice[n];
                tier1Choice.RemoveAt(n);
            }

            currentSW[2] = tier2Choice[Random.Range(0, tier2Choice.Count)];
            currentSW[3] = tier3Choice[Random.Range(0, tier3Choice.Count)];
            currentSW[4] = 8;
        }

        return skillWheel;
    }
    #endregion
}
