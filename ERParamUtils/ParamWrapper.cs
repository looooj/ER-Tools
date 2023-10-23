using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SoulsParam
{
    public class Param
    {

        private PARAM _param;
        public string ParamType;
        public string Name { get; set; }
        public List<Row> _rows = new();
        public bool Changed = false;


        public bool CheckParamdef(PARAMDEF def)
        {
            return true;// throw new NotImplementedException();
        }

        public void ApplyParamdef(PARAMDEF def)
        {
            _param.ApplyParamdef(def);
        }

        protected void Init()
        {

            ParamType = _param.ParamType;
            _rows.Clear();
            for (int i = 0; i < _param.Rows.Count; i++)
            {
                Row row = new();
                row.Init(_param.Rows[i], this);
                _rows.Add(row);
                //if (i == 0)
                //    MakeCellIndex(row);
            }

        }
        Dictionary<string, int> cellIndex = new();

        public int GetCellIndex(string key) {
            if (cellIndex.ContainsKey(key))
                return cellIndex[key];
            return -1;
        }
        public void MakeCellIndex() {
            if (_rows.Count > 0)
                MakeCellIndex(_rows[0]);
        }
        public void MakeCellIndex(Row row) {

            cellIndex.Clear();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                var cell = row.Cells[i];
                cellIndex.Add(cell.Def.InternalName, i);
            }
        }

        public static Param Read(byte[] bytes)
        {
            var _param = SoulsFile<PARAM>.Read(bytes);
            var p = new Param();
            p._param = _param;

            p.Init();

            return p;
        }

        public byte[] Write()
        {
            return _param.Write();
        }

        public class Row
        {
            private SoulsFormats.PARAM.Row _row;
            private Param _parent;
            public string Name { get => _row.Name; set => _row.Name = value; }
            public int ID { get => _row.ID; }

            public void Init(SoulsFormats.PARAM.Row row, Param param)
            {
                _row = row;
                _parent = param;
            }

            public IReadOnlyList<Cell> Cells
            {
                get
                {
                    var cells = new List<Cell>();

                    foreach (var cell in _row.Cells)
                    {
                        cells.Add(new Cell(this, cell));
                    }

                    return cells;
                }
            }

            public Param GetParam()
            {
                return _parent;
            }

        }

        public class Cell
        {
            private SoulsFormats.PARAM.Cell _cell;
            private Row _row;


            public object Value { get => _cell.Value; } // set => _cell.Value = value; }
            public PARAMDEF.Field Def => _cell.Def;

            public Cell(Row row, SoulsFormats.PARAM.Cell cell)
            {
                _cell = cell;
                _row = row;
            }

            public void SetValue(object v)
            {

                _cell.Value = v;
                _row.GetParam().Changed = true;
            }
        }

        public IReadOnlyList<Row> Rows
        {
            get => _rows;
        }



    }
}
