﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{
    public delegate bool myDelegate();
    myDelegate _question;
    INode _trueNode;
    INode _falseNode;
    public QuestionNode(myDelegate q, INode tN, INode fN)
    {
        _question = q;
        _trueNode = tN;
        _falseNode = fN;
    }
    public void Execute()
    {
        if (_question())
        {
            _trueNode.Execute();
        }
        else
        {
            _falseNode.Execute();
        }
    }
}
