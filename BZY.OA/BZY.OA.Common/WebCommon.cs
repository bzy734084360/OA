﻿using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using PanGu;
using PanGu.HighLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.Common
{
    public class WebCommon
    {
        public static List<string> PanGuSplitWord(string msg)
        {
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(msg));
            Token token = null;
            List<string> list = new List<string>();
            while ((token = tokenStream.Next()) != null)
            {
                list.Add(token.TermText());
            }
            return list;
        }
        // /创建HTMLFormatter,参数为高亮单词的前后缀
        public static string CreateHightLight(string keywords, string Content)
        {
            SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<font color=\"red\">", "</font>");
            //创建Highlighter ，输入HTMLFormatter 和盘古分词对象Semgent
            Highlighter highlighter = new Highlighter(simpleHTMLFormatter, new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 150;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keywords, Content);

        }

    }
}
