﻿#region License

/*
Copyright (c) 2013 Daniil Rodin of Buhgalteria.Kontur team of SKB Kontur

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

#endregion

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SharpRpc.Codecs
{
    public static class EmittingCodecExtensions
    {
        public static void EmitCalculateSize(this IEmittingCodec codec, ILGenerator il, int argIndex)
        {
            codec.EmitCalculateSize(il, lil => lil.Emit_Ldarg(argIndex));
        }

        public static void EmitCalculateSizeIndirect(this IEmittingCodec codec, ILGenerator il, int argIndex, Type type)
        {
            codec.EmitCalculateSize(il, lil => { lil.Emit_Ldarg(argIndex); lil.Emit(OpCodes.Ldobj, type); });
        }

        public static void EmitCalculateSize(this IEmittingCodec codec, ILGenerator il, LocalBuilder localVar)
        {
            codec.EmitCalculateSize(il, lil => lil.Emit(OpCodes.Ldloc, localVar));
        }

        public static void EmitCalculateSize(this IEmittingCodec codec, ILGenerator il, Action<ILGenerator> emitLoadParent, MethodInfo propertyGetter)
        {
            codec.EmitCalculateSize(il, lil =>
            {
                emitLoadParent(lil);
                lil.Emit(OpCodes.Call, propertyGetter);
            });
        }

        public static void EmitEncode(this IEmittingCodec codec, ILGenerator il, ILocalVariableCollection locals, int argIndex)
        {
            codec.EmitEncode(il, locals, lil => lil.Emit_Ldarg(argIndex));
        }

        public static void EmitEncodeIndirect(this IEmittingCodec codec, ILGenerator il, ILocalVariableCollection locals, int argIndex, Type type)
        {
            codec.EmitEncode(il, locals, lil => { lil.Emit_Ldarg(argIndex); lil.Emit(OpCodes.Ldobj, type); });
        }

        public static void EmitEncode(this IEmittingCodec codec, ILGenerator il, ILocalVariableCollection locals, LocalBuilder localVar)
        {
            codec.EmitEncode(il, locals, lil => lil.Emit(OpCodes.Ldloc, localVar));
        }

        public static void EmitEncode(this IEmittingCodec codec, ILGenerator il, ILocalVariableCollection locals, Action<ILGenerator> emitLoadParent, MethodInfo propertyGetter)
        {
            codec.EmitEncode(il, locals, lil =>
                {
                    emitLoadParent(lil);
                    lil.Emit(OpCodes.Call, propertyGetter);
                });
        }
    }
}
