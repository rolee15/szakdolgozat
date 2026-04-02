import React, { useState, useRef, useEffect, KeyboardEvent, FormEvent } from "react";
import * as wanakana from "wanakana";

interface WritingInputProps {
  characterType: "hiragana" | "katakana";
  onSubmit: (answer: string) => void;
  disabled?: boolean;
}

const WritingInput: React.FC<WritingInputProps> = ({ characterType, onSubmit, disabled }) => {
  const [value, setValue] = useState<string>("");
  const inputRef = useRef<HTMLInputElement>(null);

  // [6] WanaKana Contributors, "WanaKana" — https://github.com/WaniKani/WanaKana (accessed 2026-04-02)
  useEffect(() => {
    const el = inputRef.current;
    if (!el) return;
    wanakana.bind(el, { IMEMode: characterType === "hiragana" ? "toHiragana" : "toKatakana" });
    return () => {
      wanakana.unbind(el);
    };
  }, [characterType]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setValue(e.target.value);
  };

  const submit = () => {
    onSubmit(value.trim());
    setValue("");
  };

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      e.preventDefault();
      submit();
    }
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    submit();
  };

  return (
    <div className="w-full max-w-2xl mx-auto py-6">
      <form onSubmit={handleSubmit} className="flex items-center mb-6">
        <div className="relative flex-grow">
          <input
            ref={inputRef}
            type="text"
            value={value}
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            disabled={disabled}
            className="w-full px-4 py-2 border border-gray-300 rounded-l-md text-center"
            placeholder="Type romaji to convert..."
            aria-label="Writing answer"
            autoComplete="off"
          />
        </div>
        <button
          type="submit"
          disabled={disabled}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 rounded-r-md disabled:opacity-50"
          aria-label="Submit answer"
        >
          &gt;
        </button>
      </form>
    </div>
  );
};

export default WritingInput;
