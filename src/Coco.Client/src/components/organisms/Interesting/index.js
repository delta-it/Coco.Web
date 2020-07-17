import React, { Component } from "react";
import styled from "styled-components";
import { VerticalList } from "../../atoms/List";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ModuleMenuListItem } from "../../molecules/MenuList";
import { FifthHeading } from "../../atoms/Heading";
import { NavLink } from "react-router-dom";

const Root = styled.div`
  margin-bottom: 15px;
`;

const ListControl = styled(VerticalList)`
  margin: 0;
`;

const Heading = styled(FifthHeading)`
  color: ${p => p.theme.color.dark};
`;

const IconBlock = styled.span`
  display: inline-block;
  width: 25px;
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      list: [
        {
          icon: "seedling",
          text: "Vườn anh da đen",
          href: "/farms/1"
        },
        {
          icon: "piggy-bank",
          text: "Chuồng bò ông Sáu",
          href: "/farms/1"
        },
        {
          icon: "flag",
          text: "Đây là đâu tôi là ai Farm",
          href: "/farms/1"
        },
        {
          icon: "apple-alt",
          text: "Ruộng của anh Ba",
          href: "/farms/1"
        },
        {
          icon: "fish",
          text: "Ao cá bác Tư",
          href: "/farms/1"
        },
        {
          icon: "tree",
          text: "Khu vườn hạnh phúc",
          href: "/farms/1"
        },
        {
          icon: "flag",
          text: "Đây là đâu tôi là ai Farm",
          href: "/farms/1"
        },
        {
          icon: "apple-alt",
          text: "Ruộng của anh Ba",
          href: "/farms/1"
        },
        {
          icon: "fish",
          text: "Ao cá bác Tư",
          href: "/farms/1"
        },
        {
          icon: "tree",
          text: "Khu vườn hạnh phúc",
          href: "/farms/1"
        }
      ]
    };
  }

  render() {
    const { list } = this.state;
    return (
      <Root>
        <Heading>Quan Tâm</Heading>
        <ListControl>
          {list.map((item, index) => (
            <ModuleMenuListItem
              isActived={item.isActived}
              key={index}
              title={item.description}
              index={index}
            >
              <NavLink to={item.href}>
                <IconBlock>
                  <FontAwesomeIcon icon={item.icon} />
                </IconBlock>
                <span>{item.text}</span>
              </NavLink>
            </ModuleMenuListItem>
          ))}
        </ListControl>
      </Root>
    );
  }
}