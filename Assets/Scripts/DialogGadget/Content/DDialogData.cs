using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.DialogGadget {
    enum DDialogStage {
        None,
        Character,
        Content
    }
    enum DDialogParseStage {
        None,
        Dialog
    }
    public delegate void OnDialogEndDelegate();

    [Serializable]
    public class DDialogLine {
        public string characterText;
        public string contentText;

        public static DDialogLine EndOfFile() {
            DDialogLine data = new DDialogLine();
            data.characterText = "";
            data.contentText = "";
            return data;
        }
    }

    public class DDialogContent {
        public OnDialogEndDelegate onDialogEnd;

        private List<DDialogLine> dataList;
        private int curLine;

        public void ParseDialogFile(TextAsset textAsset) {
            dataList = new List<DDialogLine>();

            //TextAsset textAsset = Resources.Load<TextAsset>(filePath);
            string dialogStr = textAsset.text;
            string[] dialogList = dialogStr.Split('\n');

            foreach (string line in dialogList) {
                DDialogParseStage parseStage = DDialogParseStage.None;
                DDialogStage dialogStage = DDialogStage.None;
                Boolean isParsingString = false;
                String currentStr = null;
                DDialogLine data = null;

                foreach (char ch in line) {
                    if (isParsingString) {
                        if (ch == '"') {
                            isParsingString = false;
                            if (parseStage == DDialogParseStage.Dialog) {
                                if (dialogStage == DDialogStage.Character) {
                                    data.characterText = currentStr;
                                } else if (dialogStage == DDialogStage.Content) {
                                    data.contentText = currentStr;
                                }
                            }
                            currentStr = "";
                        } else {
                            currentStr += ch;
                        }
                        continue;
                    }
                    switch (ch) {
                        case '[':
                            parseStage = DDialogParseStage.Dialog;
                            dialogStage = DDialogStage.Character;
                            data = new DDialogLine();
                            break;
                        case ']':
                            dialogStage = DDialogStage.None;
                            break;
                        case ':':
                            if (parseStage == DDialogParseStage.Dialog && dialogStage == DDialogStage.Character) {
                                dialogStage = DDialogStage.Content;
                            }
                            break;
                        case '"':
                            isParsingString = true;
                            currentStr = "";
                            break;
                        default:
                            break;
                    }
                }

                if (data != null) {
                    dataList.Add(data);
                }
            }
            curLine = 0;
        }

        public void Reset() {
            curLine = 0;
            DDialogLine data = dataList[curLine];
        }

        public DDialogLine GetCurrentLine() {
            if (IsFinished()) {
                return DDialogLine.EndOfFile();
            }
            DDialogLine data = dataList[curLine];
            return data;
        }

        public DDialogLine GetNextLine() {
            if (curLine + 1 >= dataList.Count) {
                if (onDialogEnd != null) {
                    onDialogEnd();
                }
                return DDialogLine.EndOfFile();
            }
            curLine += 1;
            DDialogLine data = dataList[curLine];
            return data;
        }

        public List<DDialogLine> GetHistoryLineList(int count = 100) {
            List<DDialogLine> historyLineList;
            if (curLine + 1 < count)
                historyLineList = dataList.GetRange(0, curLine + 1); // gets elements 0,1,2,3)
            else
                historyLineList = dataList.GetRange(curLine - count, count); // gets elements 0,1,2,3)
            return historyLineList;
        }

        public bool IsFinished() {
            return curLine >= dataList.Count;
        }
    }
}