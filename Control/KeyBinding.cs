﻿using System;
using System.Collections.Generic;
using System.Linq;
using MaTech.Common.Algorithm;
using MaTech.Control.NativeInput;
using UnityEngine;

namespace MaTech.Control {
    public struct KeyBinding {
        public readonly KeyCode[] keyCodes;

        public KeyBinding(params KeyCode[] keyCodes) {
            // if the array shouldn't be shared, do it by your hands :)
            this.keyCodes = keyCodes;
        }

        public int KeyCount => keyCodes?.Length ?? 0;
        public bool IsValid => KeyCount > 0 && keyCodes.All(keyCode => keyCode != KeyCode.None);

        public KeyCode KeyCodeAt(int index) {
            if (keyCodes == null || index < 0 || index >= keyCodes.Length) return KeyCode.None;
            return keyCodes[index];
        }

        public RawKey RawKeyAt(int index) {
            if (keyCodes == null || index < 0 || index >= keyCodes.Length) return RawKey.None;
            return RawInput.ToRawKey(keyCodes[index]);
        }

        public int IndexOf(KeyCode key) {
            if (keyCodes == null) return -1;
            for (int i = 0; i < keyCodes.Length; i++) {
                if (keyCodes[i] == key) return i;
            }

            return -1;
        }

        public int[] ToArray() {
            var ret = new int[keyCodes.Length];
            for (int i = 0; i < keyCodes.Length; i++) {
                ret[i] = (int) keyCodes[i];
            }

            return ret;
        }
    
    }
}