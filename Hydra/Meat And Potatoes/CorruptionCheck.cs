﻿using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace UltraTelephone.Hydra
{
    public static class CorruptionCheck
    {
        public static byte[] HeheheHa = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 32, 0, 0, 0, 32, 8, 2, 0, 0, 0, 252, 24, 237, 163, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 11, 19, 0, 0, 11, 19, 1, 0, 154, 156, 24, 0, 0, 5, 209, 105, 84, 88, 116, 88, 77, 76, 58, 99, 111, 109, 46, 97, 100, 111, 98, 101, 46, 120, 109, 112, 0, 0, 0, 0, 0, 60, 63, 120, 112, 97, 99, 107, 101, 116, 32, 98, 101, 103, 105, 110, 61, 34, 239, 187, 191, 34, 32, 105, 100, 61, 34, 87, 53, 77, 48, 77, 112, 67, 101, 104, 105, 72, 122, 114, 101, 83, 122, 78, 84, 99, 122, 107, 99, 57, 100, 34, 63, 62, 32, 60, 120, 58, 120, 109, 112, 109, 101, 116, 97, 32, 120, 109, 108, 110, 115, 58, 120, 61, 34, 97, 100, 111, 98, 101, 58, 110, 115, 58, 109, 101, 116, 97, 47, 34, 32, 120, 58, 120, 109, 112, 116, 107, 61, 34, 65, 100, 111, 98, 101, 32, 88, 77, 80, 32, 67, 111, 114, 101, 32, 53, 46, 54, 45, 99, 49, 52, 53, 32, 55, 57, 46, 49, 54, 51, 52, 57, 57, 44, 32, 50, 48, 49, 56, 47, 48, 56, 47, 49, 51, 45, 49, 54, 58, 52, 48, 58, 50, 50, 32, 32, 32, 32, 32, 32, 32, 32, 34, 62, 32, 60, 114, 100, 102, 58, 82, 68, 70, 32, 120, 109, 108, 110, 115, 58, 114, 100, 102, 61, 34, 104, 116, 116, 112, 58, 47, 47, 119, 119, 119, 46, 119, 51, 46, 111, 114, 103, 47, 49, 57, 57, 57, 47, 48, 50, 47, 50, 50, 45, 114, 100, 102, 45, 115, 121, 110, 116, 97, 120, 45, 110, 115, 35, 34, 62, 32, 60, 114, 100, 102, 58, 68, 101, 115, 99, 114, 105, 112, 116, 105, 111, 110, 32, 114, 100, 102, 58, 97, 98, 111, 117, 116, 61, 34, 34, 32, 120, 109, 108, 110, 115, 58, 120, 109, 112, 61, 34, 104, 116, 116, 112, 58, 47, 47, 110, 115, 46, 97, 100, 111, 98, 101, 46, 99, 111, 109, 47, 120, 97, 112, 47, 49, 46, 48, 47, 34, 32, 120, 109, 108, 110, 115, 58, 120, 109, 112, 77, 77, 61, 34, 104, 116, 116, 112, 58, 47, 47, 110, 115, 46, 97, 100, 111, 98, 101, 46, 99, 111, 109, 47, 120, 97, 112, 47, 49, 46, 48, 47, 109, 109, 47, 34, 32, 120, 109, 108, 110, 115, 58, 115, 116, 69, 118, 116, 61, 34, 104, 116, 116, 112, 58, 47, 47, 110, 115, 46, 97, 100, 111, 98, 101, 46, 99, 111, 109, 47, 120, 97, 112, 47, 49, 46, 48, 47, 115, 84, 121, 112, 101, 47, 82, 101, 115, 111, 117, 114, 99, 101, 69, 118, 101, 110, 116, 35, 34, 32, 120, 109, 108, 110, 115, 58, 100, 99, 61, 34, 104, 116, 116, 112, 58, 47, 47, 112, 117, 114, 108, 46, 111, 114, 103, 47, 100, 99, 47, 101, 108, 101, 109, 101, 110, 116, 115, 47, 49, 46, 49, 47, 34, 32, 120, 109, 108, 110, 115, 58, 112, 104, 111, 116, 111, 115, 104, 111, 112, 61, 34, 104, 116, 116, 112, 58, 47, 47, 110, 115, 46, 97, 100, 111, 98, 101, 46, 99, 111, 109, 47, 112, 104, 111, 116, 111, 115, 104, 111, 112, 47, 49, 46, 48, 47, 34, 32, 120, 109, 112, 58, 67, 114, 101, 97, 116, 111, 114, 84, 111, 111, 108, 61, 34, 65, 100, 111, 98, 101, 32, 80, 104, 111, 116, 111, 115, 104, 111, 112, 32, 67, 67, 32, 50, 48, 49, 57, 32, 40, 87, 105, 110, 100, 111, 119, 115, 41, 34, 32, 120, 109, 112, 58, 67, 114, 101, 97, 116, 101, 68, 97, 116, 101, 61, 34, 50, 48, 50, 51, 45, 48, 49, 45, 49, 53, 84, 48, 54, 58, 51, 54, 58, 50, 51, 45, 48, 56, 58, 48, 48, 34, 32, 120, 109, 112, 58, 77, 101, 116, 97, 100, 97, 116, 97, 68, 97, 116, 101, 61, 34, 50, 48, 50, 51, 45, 48, 49, 45, 49, 53, 84, 48, 54, 58, 51, 54, 58, 50, 51, 45, 48, 56, 58, 48, 48, 34, 32, 120, 109, 112, 58, 77, 111, 100, 105, 102, 121, 68, 97, 116, 101, 61, 34, 50, 48, 50, 51, 45, 48, 49, 45, 49, 53, 84, 48, 54, 58, 51, 54, 58, 50, 51, 45, 48, 56, 58, 48, 48, 34, 32, 120, 109, 112, 77, 77, 58, 73, 110, 115, 116, 97, 110, 99, 101, 73, 68, 61, 34, 120, 109, 112, 46, 105, 105, 100, 58, 51, 52, 101, 57, 48, 53, 102, 100, 45, 56, 97, 97, 52, 45, 53, 99, 52, 57, 45, 97, 49, 52, 102, 45, 99, 51, 53, 101, 55, 57, 53, 100, 97, 52, 54, 48, 34, 32, 120, 109, 112, 77, 77, 58, 68, 111, 99, 117, 109, 101, 110, 116, 73, 68, 61, 34, 97, 100, 111, 98, 101, 58, 100, 111, 99, 105, 100, 58, 112, 104, 111, 116, 111, 115, 104, 111, 112, 58, 49, 100, 102, 97, 52, 98, 48, 52, 45, 101, 99, 99, 57, 45, 57, 51, 52, 98, 45, 97, 102, 102, 102, 45, 57, 102, 49, 49, 51, 52, 55, 99, 54, 54, 52, 55, 34, 32, 120, 109, 112, 77, 77, 58, 79, 114, 105, 103, 105, 110, 97, 108, 68, 111, 99, 117, 109, 101, 110, 116, 73, 68, 61, 34, 120, 109, 112, 46, 100, 105, 100, 58, 57, 101, 54, 97, 49, 51, 99, 55, 45, 53, 55, 53, 102, 45, 97, 50, 52, 97, 45, 98, 97, 48, 51, 45, 50, 53, 97, 98, 54, 48, 101, 54, 49, 57, 53, 50, 34, 32, 100, 99, 58, 102, 111, 114, 109, 97, 116, 61, 34, 105, 109, 97, 103, 101, 47, 112, 110, 103, 34, 32, 112, 104, 111, 116, 111, 115, 104, 111, 112, 58, 67, 111, 108, 111, 114, 77, 111, 100, 101, 61, 34, 51, 34, 62, 32, 60, 120, 109, 112, 77, 77, 58, 72, 105, 115, 116, 111, 114, 121, 62, 32, 60, 114, 100, 102, 58, 83, 101, 113, 62, 32, 60, 114, 100, 102, 58, 108, 105, 32, 115, 116, 69, 118, 116, 58, 97, 99, 116, 105, 111, 110, 61, 34, 99, 114, 101, 97, 116, 101, 100, 34, 32, 115, 116, 69, 118, 116, 58, 105, 110, 115, 116, 97, 110, 99, 101, 73, 68, 61, 34, 120, 109, 112, 46, 105, 105, 100, 58, 57, 101, 54, 97, 49, 51, 99, 55, 45, 53, 55, 53, 102, 45, 97, 50, 52, 97, 45, 98, 97, 48, 51, 45, 50, 53, 97, 98, 54, 48, 101, 54, 49, 57, 53, 50, 34, 32, 115, 116, 69, 118, 116, 58, 119, 104, 101, 110, 61, 34, 50, 48, 50, 51, 45, 48, 49, 45, 49, 53, 84, 48, 54, 58, 51, 54, 58, 50, 51, 45, 48, 56, 58, 48, 48, 34, 32, 115, 116, 69, 118, 116, 58, 115, 111, 102, 116, 119, 97, 114, 101, 65, 103, 101, 110, 116, 61, 34, 65, 100, 111, 98, 101, 32, 80, 104, 111, 116, 111, 115, 104, 111, 112, 32, 67, 67, 32, 50, 48, 49, 57, 32, 40, 87, 105, 110, 100, 111, 119, 115, 41, 34, 47, 62, 32, 60, 114, 100, 102, 58, 108, 105, 32, 115, 116, 69, 118, 116, 58, 97, 99, 116, 105, 111, 110, 61, 34, 115, 97, 118, 101, 100, 34, 32, 115, 116, 69, 118, 116, 58, 105, 110, 115, 116, 97, 110, 99, 101, 73, 68, 61, 34, 120, 109, 112, 46, 105, 105, 100, 58, 51, 52, 101, 57, 48, 53, 102, 100, 45, 56, 97, 97, 52, 45, 53, 99, 52, 57, 45, 97, 49, 52, 102, 45, 99, 51, 53, 101, 55, 57, 53, 100, 97, 52, 54, 48, 34, 32, 115, 116, 69, 118, 116, 58, 119, 104, 101, 110, 61, 34, 50, 48, 50, 51, 45, 48, 49, 45, 49, 53, 84, 48, 54, 58, 51, 54, 58, 50, 51, 45, 48, 56, 58, 48, 48, 34, 32, 115, 116, 69, 118, 116, 58, 115, 111, 102, 116, 119, 97, 114, 101, 65, 103, 101, 110, 116, 61, 34, 65, 100, 111, 98, 101, 32, 80, 104, 111, 116, 111, 115, 104, 111, 112, 32, 67, 67, 32, 50, 48, 49, 57, 32, 40, 87, 105, 110, 100, 111, 119, 115, 41, 34, 32, 115, 116, 69, 118, 116, 58, 99, 104, 97, 110, 103, 101, 100, 61, 34, 47, 34, 47, 62, 32, 60, 47, 114, 100, 102, 58, 83, 101, 113, 62, 32, 60, 47, 120, 109, 112, 77, 77, 58, 72, 105, 115, 116, 111, 114, 121, 62, 32, 60, 47, 114, 100, 102, 58, 68, 101, 115, 99, 114, 105, 112, 116, 105, 111, 110, 62, 32, 60, 47, 114, 100, 102, 58, 82, 68, 70, 62, 32, 60, 47, 120, 58, 120, 109, 112, 109, 101, 116, 97, 62, 32, 60, 63, 120, 112, 97, 99, 107, 101, 116, 32, 101, 110, 100, 61, 34, 114, 34, 63, 62, 78, 152, 59, 1, 0, 0, 8, 178, 73, 68, 65, 84, 72, 199, 181, 86, 91, 111, 29, 87, 21, 222, 123, 207, 158, 251, 204, 185, 250, 92, 227, 28, 215, 113, 108, 7, 154, 132, 182, 73, 154, 184, 13, 85, 74, 75, 75, 82, 8, 84, 128, 0, 9, 81, 84, 81, 30, 232, 19, 47, 125, 0, 9, 241, 43, 42, 161, 182, 226, 9, 1, 121, 225, 1, 65, 169, 84, 212, 90, 73, 211, 56, 105, 108, 28, 59, 23, 199, 118, 124, 236, 99, 159, 251, 117, 142, 207, 92, 247, 204, 176, 38, 169, 250, 15, 50, 26, 143, 103, 246, 153, 89, 123, 173, 245, 125, 223, 90, 11, 15, 71, 77, 132, 16, 33, 132, 82, 10, 87, 199, 113, 48, 11, 62, 120, 255, 253, 63, 255, 233, 93, 18, 186, 138, 4, 235, 129, 53, 236, 199, 99, 122, 169, 152, 99, 193, 80, 211, 185, 87, 94, 126, 173, 84, 42, 205, 207, 207, 111, 222, 175, 205, 206, 28, 91, 184, 182, 184, 179, 91, 145, 36, 37, 12, 192, 0, 226, 121, 30, 99, 204, 135, 24, 204, 250, 190, 79, 235, 181, 54, 44, 115, 28, 167, 105, 26, 92, 121, 1, 187, 174, 11, 47, 193, 61, 14, 162, 117, 89, 22, 185, 144, 157, 56, 113, 98, 238, 217, 227, 146, 140, 103, 142, 140, 199, 245, 132, 40, 138, 201, 212, 249, 106, 181, 90, 175, 117, 194, 27, 61, 194, 5, 166, 105, 80, 42, 225, 200, 213, 232, 16, 68, 25, 140, 88, 150, 69, 175, 95, 191, 1, 207, 240, 32, 203, 50, 4, 225, 122, 35, 226, 135, 43, 43, 43, 158, 231, 9, 28, 122, 228, 5, 152, 219, 220, 220, 228, 208, 104, 246, 107, 79, 216, 110, 59, 159, 203, 72, 146, 212, 239, 247, 61, 23, 111, 111, 111, 91, 214, 200, 245, 44, 159, 133, 96, 78, 224, 85, 112, 31, 236, 248, 84, 0, 179, 224, 43, 93, 92, 190, 11, 75, 170, 36, 135, 97, 200, 24, 35, 216, 79, 232, 74, 191, 103, 16, 202, 233, 186, 174, 200, 18, 132, 228, 120, 198, 190, 51, 172, 85, 233, 193, 82, 65, 16, 195, 202, 110, 61, 68, 158, 97, 244, 153, 229, 57, 150, 57, 89, 154, 44, 229, 101, 42, 74, 152, 138, 96, 17, 188, 1, 119, 145, 31, 57, 7, 55, 4, 61, 230, 131, 214, 170, 205, 8, 97, 66, 76, 211, 132, 8, 68, 1, 103, 83, 73, 195, 216, 119, 29, 188, 143, 60, 28, 82, 78, 228, 7, 61, 123, 127, 212, 235, 84, 218, 137, 100, 140, 80, 27, 242, 67, 41, 26, 24, 253, 78, 187, 171, 200, 73, 77, 201, 36, 51, 121, 64, 207, 71, 156, 72, 163, 252, 128, 93, 129, 19, 224, 10, 16, 82, 198, 0, 123, 212, 237, 117, 27, 141, 70, 42, 149, 58, 254, 220, 233, 19, 79, 29, 61, 125, 234, 217, 118, 189, 33, 114, 88, 83, 69, 77, 228, 140, 97, 107, 100, 246, 196, 16, 165, 198, 100, 81, 102, 17, 236, 28, 25, 153, 3, 199, 71, 178, 152, 160, 36, 166, 136, 49, 204, 243, 96, 73, 34, 1, 108, 0, 142, 142, 140, 209, 141, 27, 55, 54, 54, 54, 168, 205, 60, 88, 234, 14, 140, 78, 127, 16, 75, 166, 142, 159, 60, 49, 55, 119, 154, 162, 16, 113, 190, 140, 56, 236, 7, 33, 114, 186, 205, 122, 175, 215, 200, 235, 41, 65, 196, 128, 65, 64, 12, 202, 115, 253, 222, 0, 34, 103, 1, 226, 101, 45, 166, 150, 48, 108, 233, 216, 64, 107, 112, 28, 224, 108, 84, 106, 159, 125, 177, 208, 183, 6, 248, 199, 63, 127, 19, 150, 30, 220, 95, 27, 31, 31, 191, 120, 241, 226, 169, 83, 167, 100, 77, 20, 4, 26, 83, 100, 49, 36, 161, 199, 36, 78, 254, 245, 155, 111, 52, 27, 213, 51, 199, 159, 212, 99, 210, 204, 145, 9, 202, 243, 182, 109, 131, 98, 214, 54, 215, 118, 107, 173, 87, 207, 95, 56, 51, 55, 151, 206, 102, 220, 208, 167, 132, 1, 206, 192, 154, 208, 246, 61, 102, 109, 149, 55, 40, 80, 165, 82, 169, 228, 242, 217, 239, 156, 127, 245, 197, 111, 157, 211, 181, 152, 21, 216, 156, 68, 9, 230, 9, 120, 133, 41, 115, 92, 199, 177, 150, 150, 150, 6, 213, 166, 30, 19, 131, 240, 133, 195, 51, 135, 13, 195, 132, 149, 133, 229, 235, 3, 99, 191, 217, 31, 12, 29, 235, 233, 147, 39, 166, 102, 166, 109, 219, 132, 215, 193, 99, 145, 19, 18, 217, 130, 220, 169, 115, 62, 133, 20, 15, 142, 30, 59, 246, 250, 197, 31, 76, 150, 38, 16, 9, 56, 30, 18, 0, 252, 34, 60, 130, 77, 56, 228, 7, 235, 107, 27, 142, 229, 141, 23, 114, 153, 108, 202, 180, 134, 186, 18, 75, 165, 199, 68, 77, 75, 164, 211, 217, 108, 174, 52, 62, 126, 238, 220, 203, 71, 159, 60, 102, 217, 78, 32, 168, 188, 40, 113, 132, 195, 84, 0, 11, 173, 70, 139, 190, 243, 206, 59, 65, 16, 204, 78, 207, 192, 241, 168, 102, 128, 136, 61, 198, 252, 32, 48, 65, 84, 181, 70, 183, 222, 220, 218, 218, 218, 219, 219, 83, 73, 73, 16, 149, 209, 200, 27, 141, 156, 44, 226, 117, 45, 105, 155, 229, 221, 157, 214, 253, 123, 219, 207, 156, 58, 187, 63, 114, 76, 207, 57, 244, 228, 145, 3, 133, 12, 34, 81, 232, 192, 29, 140, 30, 191, 14, 112, 223, 101, 0, 58, 14, 17, 15, 222, 35, 196, 0, 86, 12, 127, 140, 6, 65, 103, 175, 118, 233, 47, 127, 221, 94, 223, 164, 8, 11, 128, 44, 11, 7, 70, 27, 136, 61, 53, 57, 149, 206, 140, 173, 109, 110, 184, 1, 134, 2, 3, 159, 223, 190, 191, 234, 120, 46, 17, 248, 159, 252, 242, 141, 239, 127, 239, 130, 34, 112, 96, 146, 98, 180, 91, 126, 64, 224, 231, 72, 211, 62, 193, 97, 128, 113, 164, 9, 160, 0, 194, 1, 144, 1, 50, 243, 225, 135, 31, 206, 207, 127, 82, 175, 87, 97, 17, 190, 160, 68, 208, 249, 52, 10, 133, 161, 225, 192, 53, 174, 106, 144, 82, 73, 87, 59, 221, 253, 242, 94, 115, 101, 125, 107, 117, 101, 13, 146, 202, 136, 64, 4, 9, 116, 139, 69, 41, 82, 29, 232, 0, 251, 97, 196, 45, 160, 56, 71, 92, 219, 230, 69, 1, 5, 24, 136, 88, 46, 151, 7, 157, 214, 104, 96, 92, 191, 182, 48, 22, 75, 28, 42, 77, 136, 80, 191, 101, 81, 148, 36, 142, 19, 174, 92, 254, 124, 167, 182, 167, 39, 19, 173, 118, 215, 69, 1, 213, 100, 230, 161, 200, 6, 39, 6, 126, 128, 66, 144, 163, 72, 151, 151, 151, 19, 137, 196, 204, 248, 161, 175, 178, 246, 168, 222, 194, 63, 40, 168, 80, 74, 21, 73, 153, 158, 158, 214, 20, 85, 64, 100, 107, 125, 195, 232, 244, 150, 86, 86, 193, 3, 72, 8, 47, 128, 220, 12, 37, 22, 23, 4, 105, 56, 28, 196, 50, 153, 66, 225, 64, 177, 80, 2, 138, 7, 8, 182, 2, 189, 98, 250, 171, 183, 222, 154, 157, 153, 249, 237, 219, 191, 57, 121, 242, 36, 20, 25, 215, 181, 35, 48, 188, 224, 206, 218, 234, 199, 31, 127, 108, 89, 142, 66, 57, 16, 224, 119, 47, 188, 6, 72, 196, 180, 184, 209, 25, 2, 235, 121, 136, 65, 83, 26, 59, 213, 245, 242, 131, 76, 33, 127, 229, 202, 213, 127, 126, 244, 175, 86, 191, 93, 173, 238, 238, 236, 236, 128, 67, 152, 64, 107, 224, 16, 22, 57, 22, 226, 59, 183, 86, 64, 183, 223, 126, 233, 37, 1, 186, 23, 10, 162, 74, 50, 26, 205, 255, 247, 211, 247, 222, 253, 192, 183, 1, 114, 174, 213, 232, 108, 174, 151, 61, 219, 79, 37, 178, 161, 199, 13, 13, 195, 177, 93, 76, 184, 125, 195, 172, 214, 26, 139, 203, 183, 22, 23, 22, 187, 253, 110, 119, 48, 216, 120, 176, 117, 246, 236, 11, 135, 15, 207, 132, 40, 32, 136, 51, 122, 6, 37, 128, 40, 20, 158, 224, 203, 243, 81, 126, 64, 165, 255, 249, 247, 71, 206, 190, 21, 83, 99, 7, 138, 19, 0, 252, 118, 185, 38, 113, 119, 154, 181, 62, 84, 216, 122, 189, 108, 218, 150, 172, 107, 189, 254, 112, 123, 111, 215, 67, 129, 53, 24, 146, 144, 151, 169, 196, 60, 31, 206, 16, 242, 15, 86, 131, 135, 249, 126, 236, 253, 128, 185, 80, 244, 67, 89, 83, 3, 140, 162, 211, 245, 120, 76, 10, 153, 236, 145, 195, 211, 151, 63, 250, 212, 15, 195, 124, 38, 158, 75, 21, 170, 149, 106, 181, 82, 95, 93, 190, 3, 109, 99, 48, 24, 0, 254, 192, 189, 124, 177, 0, 93, 112, 118, 106, 106, 187, 186, 21, 2, 224, 219, 195, 76, 42, 153, 74, 196, 125, 228, 64, 86, 200, 67, 239, 41, 164, 155, 195, 24, 62, 120, 180, 33, 244, 66, 215, 245, 142, 30, 59, 66, 127, 241, 83, 224, 232, 229, 79, 46, 87, 234, 149, 169, 169, 169, 231, 14, 205, 117, 219, 61, 189, 165, 131, 221, 71, 205, 21, 104, 61, 49, 249, 4, 52, 200, 124, 62, 127, 233, 31, 173, 190, 185, 47, 171, 202, 243, 231, 94, 136, 167, 83, 84, 18, 9, 11, 252, 135, 54, 169, 170, 170, 182, 105, 126, 21, 17, 80, 215, 243, 66, 219, 182, 14, 62, 113, 240, 204, 217, 185, 207, 174, 94, 109, 247, 154, 233, 108, 234, 155, 207, 63, 223, 239, 246, 155, 205, 38, 112, 26, 94, 131, 13, 30, 198, 207, 193, 126, 99, 99, 99, 205, 247, 90, 229, 234, 110, 178, 152, 59, 250, 212, 55, 14, 78, 68, 189, 1, 177, 224, 203, 20, 197, 84, 10, 13, 154, 23, 9, 156, 158, 111, 71, 35, 141, 0, 35, 13, 131, 30, 215, 237, 84, 4, 209, 13, 109, 241, 111, 151, 254, 14, 228, 251, 209, 207, 126, 248, 234, 153, 87, 60, 147, 185, 204, 7, 19, 146, 166, 66, 220, 171, 171, 171, 191, 251, 227, 31, 106, 195, 154, 148, 18, 139, 7, 199, 110, 222, 93, 124, 122, 247, 153, 89, 117, 150, 231, 66, 40, 21, 8, 51, 26, 34, 215, 99, 166, 31, 216, 204, 183, 32, 107, 204, 119, 35, 145, 113, 184, 221, 110, 252, 111, 249, 102, 46, 55, 198, 17, 165, 185, 87, 251, 226, 214, 53, 27, 155, 170, 172, 106, 162, 206, 139, 50, 140, 85, 64, 158, 88, 44, 6, 25, 222, 216, 217, 224, 100, 94, 82, 128, 68, 34, 84, 151, 78, 167, 3, 57, 132, 74, 3, 67, 22, 0, 6, 234, 241, 84, 13, 74, 89, 56, 50, 251, 240, 1, 197, 132, 23, 8, 140, 58, 157, 110, 11, 250, 12, 38, 161, 158, 144, 4, 154, 91, 95, 187, 127, 123, 253, 110, 200, 66, 153, 87, 56, 42, 192, 6, 62, 137, 122, 58, 104, 115, 56, 50, 242, 233, 113, 65, 145, 119, 246, 118, 21, 69, 137, 102, 150, 135, 93, 19, 142, 104, 164, 155, 158, 46, 66, 105, 148, 36, 94, 215, 213, 100, 50, 174, 234, 113, 230, 56, 181, 221, 202, 234, 237, 229, 181, 245, 123, 84, 164, 147, 19, 83, 84, 20, 70, 142, 53, 150, 76, 65, 233, 48, 125, 111, 104, 155, 240, 232, 49, 175, 56, 126, 32, 145, 74, 232, 48, 86, 166, 226, 12, 67, 119, 38, 112, 3, 212, 202, 101, 51, 48, 94, 48, 15, 6, 42, 231, 177, 235, 128, 155, 60, 144, 8, 3, 127, 175, 86, 135, 124, 65, 200, 7, 242, 5, 223, 177, 55, 214, 238, 46, 221, 92, 170, 238, 238, 141, 23, 138, 233, 120, 210, 26, 237, 51, 215, 73, 196, 83, 185, 124, 129, 10, 162, 231, 49, 65, 16, 128, 126, 80, 190, 138, 197, 34, 36, 54, 91, 200, 3, 236, 138, 166, 42, 42, 76, 133, 189, 98, 62, 159, 214, 180, 225, 192, 216, 217, 220, 226, 190, 62, 153, 3, 178, 89, 140, 193, 60, 10, 16, 169, 130, 72, 2, 118, 255, 222, 157, 155, 55, 23, 99, 48, 16, 104, 26, 180, 124, 223, 103, 146, 36, 2, 191, 40, 224, 67, 97, 164, 128, 21, 31, 76, 103, 178, 89, 208, 65, 58, 157, 22, 100, 9, 68, 0, 37, 66, 213, 180, 102, 163, 161, 41, 138, 128, 194, 133, 171, 159, 95, 153, 159, 199, 191, 127, 251, 117, 168, 219, 88, 143, 63, 40, 87, 150, 110, 173, 116, 155, 237, 124, 60, 198, 49, 214, 232, 13, 226, 241, 120, 132, 82, 24, 205, 191, 128, 216, 190, 19, 64, 165, 9, 65, 40, 46, 50, 29, 27, 83, 232, 8, 60, 132, 18, 141, 114, 36, 26, 214, 33, 7, 136, 80, 207, 180, 179, 169, 180, 172, 10, 129, 101, 198, 101, 249, 255, 94, 252, 145, 153, 22, 92, 199, 65, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
        public static void Check()
        {
            if (!TrueCheck() && !TelephoneData.Data.coconutted)
            {
                Debug.LogError("Missing file coconut.png");
                TelephoneData.Data.coconutted = true;
                TelephoneData.SaveData();
                Application.Quit();
            }
            else
            {
                TryDownloadCoconut();
            }
        }

        private static bool TrueCheck()
        {
            string path = Path.Combine(BestUtilityEverCreated.TextureLoader.GetTextureFolder(), "coconut.png");

            if (!File.Exists(path))
            {
                return false;
            }

            return true;
        }

        private static string coconutUrl = "https://hyperworld.org/images/extra/coconut.png";

        private static void TryDownloadCoconut()
        {
            if (!TrueCheck())
            {
                TelephoneData.Data.coconutMurders = Mathf.Min(TelephoneData.Data.coconutMurders+1,10);
                TelephoneData.SaveData();
                GameObject coconutInstaller = new GameObject("CoconutInstaller");
                DestroyAfterTime bruh = coconutInstaller.AddComponent<DestroyAfterTime>();
                bruh.timeLeft = 32.0f;
                bruh.StartCoroutine(DownloadCoconut(coconutUrl));
            }
        }

        //yoink :)
        private static IEnumerator DownloadCoconut(string url)
        {
            SimpleLogger.Log("Trying to download coconut.png");

            if(TelephoneData.Data.coconutMurders > 2)
            {
                url = "https://hyperworld.org/images/extra/coconut1.png";
            }

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    SimpleLogger.Log("WE COULDNT DOWNLOAD THE COCONUT.");
                    Jumpscare.Scare();
                    SimpleLogger.Log(www.error);
                }
                else
                {
                    Texture2D coconut = DownloadHandlerTexture.GetContent(www);
                    BestUtilityEverCreated.TextureLoader.AddTextureToCache(coconut);
                    Jumpscare.ScareWithTexture(coconut);
                    byte[] bytes = coconut.EncodeToPNG();
                    for(int i=0;i < TelephoneData.Data.coconutMurders+1; i++)
                    {
                        string path = Path.Combine(BestUtilityEverCreated.TextureLoader.GetTextureFolder(), $"coconut{((i > 1) ? ("(" + (i-1).ToString() + ")") : "")}.png");
                        if (!File.Exists(path))
                        {
                            File.WriteAllBytes(path, bytes);
                        }
                    }
                }
            }
        }
    }
}
