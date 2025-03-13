import { useRef } from "react";
import { toKana } from 'wanakana';

type Props = {
  value: string;
  onChange: (value: string) => void;
};

const RomajiInputField = ({ onChange }: Props) => {
  const inputFieldRef = useRef<HTMLInputElement>(null);

  toKana.bind(inputFieldRef);

  return (
    <div>
      <input type="text" ref={inputFieldRef} />
      <button onClick={() => onChange("")}>Submit answer</button>
    </div>
  );
};
export default RomajiInputField;
