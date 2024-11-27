import React from "react";
import "../styles/dropdown.css"

const Dropdown = ({ options, onChange }) => {
  return (
    <div className="dropdown">
      <select onChange={onChange}>
        {options.map((option, index) => (
          <option key={index} value={option}>
            {option}
          </option>
        ))}
      </select>
    </div>
  );
};

export default Dropdown;
