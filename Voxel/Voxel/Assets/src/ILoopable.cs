﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoopable {
    void Start();
    void Update();

    void OnApplicationQuit();
}
