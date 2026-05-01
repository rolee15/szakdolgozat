import KanaButton from "./KanaButton";

interface GridSectionProps {
  title: string;
  grid: (string | null)[][];
  rowLabels: string[];
  colHeaders: string[];
  charMap: Map<string, KanaCharacter>;
  type: "hiragana" | "katakana";
  className?: string;
}

const GridSection = ({ title, grid, rowLabels, colHeaders, charMap, type, className }: GridSectionProps) => (
  <div className={className}>
    <h2 className="text-xl font-semibold mb-3">{title}</h2>
    <div className="space-y-2">
      <div className="flex space-x-2">
        <div className="w-10" aria-hidden="true" />
        {colHeaders.map((h) => (
          <div key={h} className="w-20 flex items-center justify-center text-sm font-medium text-gray-400">
            {h}
          </div>
        ))}
      </div>
      {grid.map((row, rowIndex) => (
        <div key={rowIndex} className="flex items-start space-x-2">
          <div className="w-10 h-[88px] flex items-center justify-center text-sm font-medium text-gray-400">
            {rowLabels[rowIndex]}
          </div>
          {row.map((romanization, colIndex) => {
            const char = romanization ? charMap.get(romanization) : null;
            if (!char) {
              return (
                <div key={colIndex} className="flex flex-col items-center" aria-hidden="true">
                  <div className="w-20 h-20 rounded" />
                  <div className="w-20 h-2 mt-1" />
                </div>
              );
            }
            return (
              <KanaButton
                key={char.character}
                type={type}
                character={char.character}
                romanization={char.romanization}
                proficiency={char.proficiency}
              />
            );
          })}
        </div>
      ))}
    </div>
  </div>
);

export default GridSection;
