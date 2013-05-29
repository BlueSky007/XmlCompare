using System;
using System.Collections;

namespace DifferenceEngine
{
    public enum DiffEngineLevel
    {
        FastImperfect,
        Medium,
        SlowPerfect
    }

    public class DiffEngine
    {
        private IDiffList _source;
        private IDiffList _dest;
        private ArrayList _matchList;

        private DiffEngineLevel _level;

        private DiffStateList _stateList;

        public DiffEngine()
        {
            _source = null;
            _dest = null;
            _matchList = null;
            _stateList = null;
            _level = DiffEngineLevel.FastImperfect;
        }

        //比如当前新文件的当前行为第1行，与老版本的每一行进行比较。
        private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (_dest.GetByIndex(destIndex + matchCount).CompareTo(_source.GetByIndex(sourceIndex + matchCount)) != 0)
                {
                    break;//如果比较：两个行不相同 就跳出循环
                }
            }
            return matchCount;
        }

        private void GetLongestSourceMatch(DiffState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd)
        {
            int maxDestLength = (destEnd - destIndex) + 1;//新版本最大行数
            int curLength = 0;
            int curBestLength = 0;
            int curBestIndex = -1;
            int maxLength = 0;
            for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++)
            {
                maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
                if (maxLength <= curBestLength)
                {
                    //No chance to find a longer one any more
                    break;
                }
                //比如当前新文件的当前行为第1行，1-9行相同Unchanged，第10行不匹配Different，返回 10-0 = 10 相同数目......
                curLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
                if (curLength > curBestLength)
                {
                    //This is the best match so far
                    curBestIndex = sourceIndex;
                    curBestLength = curLength;//当前有10行匹配相同
                }
                //jump over the match，跳出匹配项
                sourceIndex += curBestLength; //将索引增加匹配的个数，比较下一行
            }
            //设置当前行的状态：
            if (curBestIndex == -1)//如果从第一行找到最后一行，没有相同项目。设置每个行为不匹配状态
            {
                curItem.SetNoMatch();
            }
            else
            {
                curItem.SetMatch(curBestIndex, curBestLength);
            }
        }

        private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd)
        {
            int curBestIndex = -1;         //当前匹配项
            int curBestLength = -1;        //当前匹配状态（匹配、不匹配）
            int maxPossibleDestLength = 0; //最大行数
            DiffState curItem = null;
            DiffState bestItem = null;

            //循环新版本所有行
            for (int destIndex = destStart; destIndex <= destEnd; destIndex++)
            {
                maxPossibleDestLength = (destEnd - destIndex) + 1;
                if (maxPossibleDestLength <= curBestLength)
                {
                    break;
                }
                //循环新文件的当前行状态对象：
                curItem = _stateList.GetByIndex(destIndex);


                if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength))
                {
                    //如果装填不知道，返回true
                    //设置当前行的状态
                    //recalc new best length since it isn't valid or has never been done.
                    GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);
                }
                //如果新文件当前行和老文件匹配（相同）
                if (curItem.Status == DiffStatus.Matched)
                {
                    switch (_level)
                    {
                        case DiffEngineLevel.FastImperfect:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            //Jump over the match 
                            destIndex += curItem.Length - 1;    //当前第9行不相同，从第9行开始往下比较下一个不匹配项
                            break;
                    }
                }
            }
            if (curBestIndex < 0)
            {
                //we are done - there are no matches in this span
            }
            else  //如果新文件当前行和老文件匹配（相同）
            {
                int sourceIndex = bestItem.StartIndex;
                _matchList.Add(DiffResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
                if (destStart < curBestIndex)
                {
                    //Still have more lower destination data
                    if (sourceStart < sourceIndex)
                    {
                        //Still have more lower source data
                        // Recursive call to process lower indexes
                        ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
                    }
                }
                int upperDestStart = curBestIndex + curBestLength;
                int upperSourceStart = sourceIndex + curBestLength;
                if (destEnd > upperDestStart)
                {
                    //we still have more upper dest data
                    if (sourceEnd > upperSourceStart)
                    {
                        //set still have more upper source data
                        // Recursive call to process upper indexes
                        ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
                    }
                }
            }
        }

        public double ProcessDiff(IDiffList source, IDiffList destination, DiffEngineLevel level)
        {
            _level = level;
            return ProcessDiff(source, destination);
        }

        public double ProcessDiff(IDiffList source, IDiffList destination)
        {
            DateTime dt = DateTime.Now;
            _source = source;
            _dest = destination;
            _matchList = new ArrayList();

            int dcount = _dest.Count();
            int scount = _source.Count();


            if ((dcount > 0) && (scount > 0))
            {
                _stateList = new DiffStateList(dcount);//初始化状态数组，数组长度为新版本行数
                ProcessRange(0, dcount - 1, 0, scount - 1);
            }

            TimeSpan ts = DateTime.Now - dt;
            return ts.TotalSeconds;
        }

        //该方法将不同行细分为：修改、删除、新增类型；
        private bool AddChanges(
            ArrayList report,
            int curDest,
            int nextDest,
            int curSource,
            int nextSource)
        {
            bool retval = false;
            int diffDest = nextDest - curDest;//下一个区域的索引减去当前区域索引（区域总共多少行？）
            int diffSource = nextSource - curSource;
            int minDiff = 0;
            if (diffDest > 0)
            {
                if (diffSource > 0)
                {
                    minDiff = Math.Min(diffDest, diffSource);
                    //curDest---新版本文件中区域块第一行的索引
                    //curSource --- 老版本文件中区域块第一行索引
                    //minDiff ---区域块中行数目
                    report.Add(DiffResultSpan.CreateReplace(curDest, curSource, minDiff));
                    //比如老版本10行，新版本5行；合并后10行不同，前5行是修改状态，后5行是新增状态
                    //minDiff = 5；
                    
                    if (diffDest > diffSource)
                    {
                        curDest += minDiff;
                        report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
                    }
                    else//比如老版本3行，新版本5行；合并后5行不同，前3行是修改状态，后2行是删除状态
                    //minDiff = 3;
                    {
                        if (diffSource > diffDest)
                        {
                            curSource += minDiff;
                            report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
                        }
                    }
                }
                else //diffSource == 0表示老版本没有行，新版本有，表示新增行
                {
                    report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest));
                }
                retval = true;
            }
            else
            {
                if (diffSource > 0)
                {
                    report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource));
                    retval = true;
                }
            }
            return retval;
        }

        //添加不同行的区域块，从第一行到最后一行，相邻的行状态相同为一个区域块
        //DiffReport返回参数解释：
        //int DestIndex   ---区域块第一行在新版本文件中的原来的索引
        //SourceIndex     ---区域块第一行在老版本文件中的原来的索引
        //int Length      ---当前区域块中的行的个数
        //Status          ---改区域块中所有行共同的状态（Add/Delete/Modify）
        public ArrayList DiffReport()
        {
            ArrayList resultList = new ArrayList();//返回值
            int dcount = _dest.Count();
            int scount = _source.Count();

            //处理空文件
            if (dcount == 0)
            {
                if (scount > 0)
                {
                    resultList.Add(DiffResultSpan.CreateDeleteSource(0, scount));
                }
                return resultList;
            }
            else
            {
                if (scount == 0)
                {
                    resultList.Add(DiffResultSpan.CreateAddDestination(0, dcount));
                    return resultList;
                }
            }

            //_matchList添加了相同行和不同行区域块，但是不同行又分为：新增、删除、修改三个状态没有区分
            _matchList.Sort();
            int curNewIndex = 0;
            int curOldIndex = 0;
            DiffResultSpan last = null;

            //Process each match record
            foreach (DiffResultSpan drs in _matchList)
            {
                if ((!AddChanges(resultList, curNewIndex, drs.DestIndex, curOldIndex, drs.SourceIndex)) &&
                    (last != null))
                {
                    last.AddLength(drs.Length);
                }
                else
                {
                    resultList.Add(drs);
                }
                curNewIndex = drs.DestIndex + drs.Length;//新版本当前区域块第一行的索引。
                curOldIndex = drs.SourceIndex + drs.Length;
                last = drs;
            }

            //Process any tail end data
            AddChanges(resultList, curNewIndex, dcount, curOldIndex, scount);

            return resultList;
        }
    }
}
