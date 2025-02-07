import api from "@services/kanaService";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const CharacterDetail = () => {
  const { type, character } = useParams();
  const [charData, setCharData] = useState<KanaCharacter | null>(null);
  const [examples, setExamples] = useState<Example[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!type || !character) return;

      try {
        const [charDetail, exampleData] = await Promise.all([
          api.getCharacterDetail(type, character),
          api.getExamples(type, character)
        ]);

        setCharData(charDetail);
        setExamples(exampleData);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load character data');
      }
    };

    fetchData();
  }, [type, character]);

  if (error) {
    return (
      <div className="p-4 text-red-500">
        Error: {error}
      </div>
    );
  }

  if (!charData) return <div className="p-4">Loading...</div>;

  return (
    <div className="max-w-2xl mx-auto p-4">
      <div className="px-16 bg-indigo-950 rounded-lg shadow-lg p-6">
        <div className="text-center mb-8">
          <div className="text-white text-8xl font-bold mb-4">{charData.character}</div>
          <div className="text-white text-2xl ">{charData.romanization}</div>
          <div className="mt-4 rounded-full px-4 py-2 inline-block">
            Proficiency: {charData.proficiency}%
          </div>
        </div>

        <div className="flex justify-center space-y-6">

          <section>
            <h2 className="text-center text-xl font-bold mb-3">Example Words</h2>
            <div className="py-4 grid grid-cols-1 gap-1">
              {examples.map((example, index) => (
                <div key={index} className="p-3 rounded-lg">
                  {example.word} ({example.romanization}) - {example.meaning}
                </div>
              ))}
            </div>
          </section>
        </div>
      </div>
    </div>
  );
};

export default CharacterDetail;
