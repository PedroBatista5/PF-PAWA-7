import React, { useState } from "react";
import Dropdown from "../components/dropdown.js";
import Input from "./input.js";
import "../styles/searchpage.css"

const Searchpage = () => {

  const [search, setSearch] = useState("");

  return (
    <div className="search-container">
      <Dropdown
        options={["Filtros", "Opção 1", "Opção 2", "Opção 3"]}
      />

      <Input
        type="text"
        placeholder="Pesquisa"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
    </div>
  );
};

export default Searchpage;
